# PostgreSQL Scripts

## Table Creation Scripts

### ENUMs

```sql
CREATE TYPE membership_type_enum  AS ENUM ('Basic', 'Premium', 'Student');
CREATE TYPE copy_status_enum      AS ENUM ('available', 'borrowed', 'unavailable', 'damaged');
CREATE TYPE borrowing_status_enum AS ENUM ('active', 'returned');
CREATE TYPE fine_status_enum      AS ENUM ('unpaid', 'paid', 'waived');
```

### 1. Membership Type

No dependencies.

```sql
CREATE TABLE membership_type (
    membership_type_id    SERIAL               PRIMARY KEY,
    membership_type_name  membership_type_enum NOT NULL,
    max_active_borrowings INT                  NOT NULL CHECK (max_active_borrowings > 0),
    max_borrow_days       INT                  NOT NULL CHECK (max_borrow_days > 0)
);
```

### 2. Member

Depends on: `membership_type`

```sql
CREATE TABLE member (
    member_id            SERIAL          PRIMARY KEY,
    member_name          VARCHAR(100)    NOT NULL,
    member_email         VARCHAR(150)    NOT NULL UNIQUE,
    member_phone         VARCHAR(15)     NOT NULL UNIQUE,
    membership_type_id   INT             NOT NULL,
    is_active            BOOLEAN         NOT NULL DEFAULT TRUE,
    created_at           TIMESTAMP       NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_member_membership_type
        FOREIGN KEY (membership_type_id)
        REFERENCES membership_type (membership_type_id)
);
```

### 3. Book Category

No dependencies.

```sql
CREATE TABLE book_category (
    book_category_id    SERIAL       PRIMARY KEY,
    book_category_name  VARCHAR(100) NOT NULL UNIQUE
);
```

### 4. Book

Depends on: `book_category`

```sql
CREATE TABLE book (
    book_id           SERIAL       PRIMARY KEY,
    book_title        VARCHAR(200) NOT NULL,
    book_author       VARCHAR(150) NOT NULL,
    book_category_id  INT          NOT NULL,

    CONSTRAINT fk_book_category
        FOREIGN KEY (book_category_id)
        REFERENCES book_category (book_category_id)
);
```

### 5. Book Copy

Depends on: `book`

```sql
CREATE TABLE book_copy (
    book_copy_id  SERIAL           PRIMARY KEY,
    book_id       INT              NOT NULL,
    copy_status   copy_status_enum NOT NULL DEFAULT 'available',

    CONSTRAINT fk_book_copy_book
        FOREIGN KEY (book_id)
        REFERENCES book (book_id)
);
```

### 6. Borrowing

Depends on: `member`, `book_copy`

```sql
CREATE TABLE borrowing (
    borrowing_id      SERIAL                NOT NULL PRIMARY KEY,
    member_id         INT                   NOT NULL,
    book_copy_id      INT                   NOT NULL,
    borrow_date       DATE                  NOT NULL DEFAULT CURRENT_DATE,
    due_date          DATE                  NOT NULL,
    return_date       DATE                  NULL,
    borrowing_status  borrowing_status_enum NOT NULL DEFAULT 'active',

    CONSTRAINT fk_borrowing_member
        FOREIGN KEY (member_id)
        REFERENCES member (member_id),

    CONSTRAINT fk_borrowing_book_copy
        FOREIGN KEY (book_copy_id)
        REFERENCES book_copy (book_copy_id),

    CONSTRAINT chk_due_date_after_borrow
        CHECK (due_date > borrow_date),

    CONSTRAINT chk_return_date_after_borrow
        CHECK (return_date IS NULL OR return_date >= borrow_date)
);
```

### 7. Fine

Depends on: `borrowing`

```sql
CREATE TABLE fine (
    fine_id           SERIAL           PRIMARY KEY,
    borrowing_id      INT              NOT NULL UNIQUE,
    fine_amount       NUMERIC(10, 2)   NOT NULL CHECK (fine_amount > 0),
    fine_paid_status  fine_status_enum NOT NULL DEFAULT 'unpaid',

    CONSTRAINT fk_fine_borrowing
        FOREIGN KEY (borrowing_id)
        REFERENCES borrowing (borrowing_id)
);
```

### 8. Fine Payment

Depends on: `fine`, `member`

```sql
CREATE TABLE fine_payment (
    payment_id        SERIAL         PRIMARY KEY,
    fine_id           INT            NOT NULL,
    member_id         INT            NOT NULL,
    fine_paid_amount  NUMERIC(10, 2) NOT NULL CHECK (fine_paid_amount > 0),
    paid_date         TIMESTAMP      NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_fine_payment_fine
        FOREIGN KEY (fine_id)
        REFERENCES fine (fine_id),

    CONSTRAINT fk_fine_payment_member
        FOREIGN KEY (member_id)
        REFERENCES member (member_id)
);
```

### Indexes

```sql
CREATE INDEX idx_member_email         ON member (member_email);
CREATE INDEX idx_member_phone         ON member (member_phone);
CREATE INDEX idx_book_title           ON book (book_title);
CREATE INDEX idx_book_author          ON book (book_author);
CREATE INDEX idx_book_category        ON book (book_category_id);
CREATE INDEX idx_book_copy_book       ON book_copy (book_id);
CREATE INDEX idx_book_copy_status     ON book_copy (copy_status);
CREATE INDEX idx_borrowing_member     ON borrowing (member_id);
CREATE INDEX idx_borrowing_status     ON borrowing (borrowing_status);
CREATE INDEX idx_fine_paid_status     ON fine (fine_paid_status);
CREATE INDEX idx_fine_payment_member  ON fine_payment (member_id);
```

---

## PostgreSQL Functions

### 1. `calculate_member_fine(member_id)`

Returns total unpaid fine amount for a member.

```sql
CREATE OR REPLACE FUNCTION calculate_member_fine(p_member_id INT)
RETURNS NUMERIC(10, 2)
LANGUAGE plpgsql
AS $$
DECLARE
    total_fine NUMERIC(10, 2);
BEGIN
    SELECT COALESCE(SUM(f.fine_amount), 0)
    INTO total_fine
    FROM fine f
    JOIN borrowing b ON f.borrowing_id = b.borrowing_id
    WHERE b.member_id = p_member_id
      AND f.fine_paid_status = 'unpaid';

    RETURN total_fine;
END;
$$;
```

### 2. `get_available_books_by_category(category_id)`

Returns available books under a category.

```sql
CREATE OR REPLACE FUNCTION get_available_books_by_category(p_category_id INT)
RETURNS TABLE (
    book_id          INT,
    book_title       VARCHAR,
    book_author      VARCHAR,
    available_copies BIGINT
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        b.book_id,
        b.book_title,
        b.book_author,
        COUNT(bc.book_copy_id) AS available_copies
    FROM book b
    JOIN book_copy bc ON b.book_id = bc.book_id
    WHERE b.book_category_id = p_category_id
      AND bc.copy_status = 'available'
    GROUP BY b.book_id, b.book_title, b.book_author
    HAVING COUNT(bc.book_copy_id) > 0;
END;
$$;
```

### 3. `get_member_borrowing_summary(member_id)`

Returns active borrows, returned borrows, and total unpaid fine for a member.

```sql
CREATE OR REPLACE FUNCTION get_member_borrowing_summary(p_member_id INT)
RETURNS TABLE (
    active_borrowings   BIGINT,
    returned_borrowings BIGINT,
    total_unpaid_fine   NUMERIC(10, 2)
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        COUNT(*) FILTER (WHERE br.borrowing_status = 'active')                        AS active_borrowings,
        COUNT(*) FILTER (WHERE br.borrowing_status = 'returned')                      AS returned_borrowings,
        COALESCE(SUM(f.fine_amount) FILTER (WHERE f.fine_paid_status = 'unpaid'), 0)  AS total_unpaid_fine
    FROM borrowing br
    LEFT JOIN fine f ON br.borrowing_id = f.borrowing_id
    WHERE br.member_id = p_member_id;
END;
$$;
```
