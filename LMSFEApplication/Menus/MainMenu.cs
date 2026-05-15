using LMSBLLLibrary.Services;
using LMSDALLibrary.Repositories;
using LMSFEApplication.Helpers;

namespace LMSFEApplication.Menus
{
    public class MainMenu
    {
        private readonly MemberMenu _memberMenu;
        private readonly BookMenu _bookMenu;
        private readonly BorrowMenu _borrowMenu;
        private readonly ReturnMenu _returnMenu;
        private readonly FineMenu _fineMenu;
        private readonly ReportMenu _reportMenu;

        public MainMenu()
        {
            var memberService = new MemberService();
            var bookService = new BookService();
            var bookCopyService = new BookCopyService();
            var bookCategoryService = new BookCategoryService();
            var borrowingService = new BorrowingService();
            var returnService = new ReturnService();
            var fineService = new FineService();
            var reportService = new ReportService();
            var membershipTypeRepo = new MembershipTypeRepository();

            _memberMenu = new MemberMenu(memberService, membershipTypeRepo);
            _bookMenu = new BookMenu(bookService, bookCopyService, bookCategoryService);
            _borrowMenu = new BorrowMenu(borrowingService, memberService, bookService, bookCopyService);
            _returnMenu = new ReturnMenu(returnService, reportService, memberService);
            _fineMenu = new FineMenu(fineService, memberService);
            _reportMenu = new ReportMenu(reportService, bookCategoryService);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("COMMUNITY LIBRARY MANAGEMENT SYSTEM");
                Console.WriteLine("1. Member Management");
                Console.WriteLine("2. Book Management");
                Console.WriteLine("3. Borrow Book");
                Console.WriteLine("4. Return Book");
                Console.WriteLine("5. Fine Management");
                Console.WriteLine("6. Reports");
                Console.WriteLine("7. Exit");

                var choice = ConsoleHelper.ReadMenuChoice(1, 7);

                switch (choice)
                {
                    case 1: _memberMenu.Run(); break;
                    case 2: _bookMenu.Run(); break;
                    case 3: _borrowMenu.Run(); break;
                    case 4: _returnMenu.Run(); break;
                    case 5: _fineMenu.Run(); break;
                    case 6: _reportMenu.Run(); break;
                    case 7:
                        Console.WriteLine("Goodbye!");
                        return;
                }
            }
        }
    }
}
