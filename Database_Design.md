# Tables

## Member

member_id (PK)
member_name
member_email
member_phone
membership_type_id (FK)
is_active

## Book

book_id (PK)
book_title
book_author
book_category_id (FK)

## Book_Copy

book_copy_id (PK)
book_id (FK)
copy_status (enum available/borrowed/unavailable/damaged)

## Book_Category

book_category_id (PK)
book_category_name

## Borrowing

borrowing_id (PK)
borrow_date
due_date
book_copy_id (FK)
member_id (FK)
borrowing_status
return_date

## Fine

fine_id (PK)
borrowing_id (FK)
fine_amount
fine_paid_status

## Membership Type

membership_type_id (PK)
membership_type_name (enum Basic,Premium, Student)
max_active_borrowings
max_borrow_days

## Fine_Payment

payment_id (PK)
paid_date
fine_paid_amount
fine_id (FK)
member_id (FK)

