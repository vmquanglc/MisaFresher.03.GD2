using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Model;
using MISA.WebFresher2023.Demo.DL.Repository;
using NSubstitute;

namespace MISA.WebFresher2023.Demo.UnitTests.Service
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        private readonly static EmployeeDto _employeeOutPage = new()
        {
            EmployeeCode = "NV-0012",
            FullName = "Lê Đức Tiệp",
            Email = "tiep@gmail.com",
            PositionName = "Trưởng phòng",
            DepartmentName = "Phòng hành chính",
        };
        private readonly static BasePage<EmployeeDto> _employeePage = new()
        {
            TotalRecord = 3,
            Data =
            new List<EmployeeDto>(){
                _employeeOutPage,
                _employeeOutPage,
                _employeeOutPage
            }
        };
        private IEmployeeRepository employeeRepository;

        private IMapper mapper;

        private EmployeeService employeeService;
        private void InitGetPage()
        {
            employeeRepository = Substitute.For<IEmployeeRepository>();

            mapper = Substitute.For<IMapper>();

            //employeeService = new(employeeRepository, mapper);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetPageAsync_InvalidPageSize_ReturnsException(int pageSize)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize= pageSize, PageNumber=1, SearchTerm="" };


            // Act
            BadRequestException? exception = Assert.ThrowsAsync<BadRequestException>(async () => await employeeService.GetPageAsync(employeePageArgument));


            // Assert
            Assert.That(exception.UserMessage[0], Is.EqualTo(PagingUserMessage.InvalidPageSize));

            _ = employeeRepository.Received(0).GetPageAsync<EmployeeDto>(employeePageArgument);
        }

        [Test]
        [TestCase(1)]
        [TestCase(999999999)]
        public async Task GetPageAsync_ValidPageSize_ReturnsEmployeePage(int pageSize)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize = pageSize, PageNumber = 1, SearchTerm = "" };

            employeeRepository.GetPageAsync<EmployeeDto>(employeePageArgument).Returns(_employeePage);

            // Act
            BasePage<EmployeeDto> employeePage = await employeeService.GetPageAsync(employeePageArgument);

            // Assert
            // Trả về đúng định dạng
            Assert.That(_employeePage, Is.SameAs(employeePage));

            // Chỉ gọi 1 lần 
            _ = await employeeRepository.Received(1).GetPageAsync<EmployeeDto>(employeePageArgument);
        }
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetPageAsync_InvalidPageNumber_ReturnsException(int pageNumber)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize = 1, PageNumber = pageNumber, SearchTerm = "" };

            // Act
            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await employeeService.GetPageAsync(employeePageArgument));

            // Assert
            Assert.That(exception.UserMessage[0], Is.EqualTo(PagingUserMessage.InvalidPageNumber));

            _ = employeeRepository.Received(0).GetPageAsync<EmployeeDto>(employeePageArgument);
        }

        [Test]
        [TestCase(1)]
        [TestCase(999999999)]
        public async Task GetPageAsync_ValidPageNumber_ReturnsEmployeePage(int pageNumber)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize = 1, PageNumber = pageNumber, SearchTerm = "" };

            employeeRepository.GetPageAsync<EmployeeDto>(employeePageArgument).Returns(_employeePage);
            // Act
            BasePage<EmployeeDto> employeePage = await employeeService.GetPageAsync(employeePageArgument);

            // Assert
            // Trả về đúng định dạng
            Assert.That(_employeePage, Is.SameAs(employeePage));

            // Chỉ gọi 1 lần 
            _ = await employeeRepository.Received(1).GetPageAsync<EmployeeDto>(employeePageArgument);
        }

        [Test]
        [TestCase("iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii")]
        public void GetPageAsync_InvalidEmployeeSearchTerm_ReturnsException(string employeeSearchTerm)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize = 1, PageNumber = 1, SearchTerm = employeeSearchTerm };

            // Act
            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await employeeService.GetPageAsync(employeePageArgument));

            // Assert
            Assert.That(exception.UserMessage[0], Is.EqualTo(PagingUserMessage.EmployeeSearchTermTooLong));

            _ = employeeRepository.Received(0).GetPageAsync<EmployeeDto>(employeePageArgument);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("ahihi")]
        [TestCase("iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii")]
        public async Task GetPageAsync_ValidEmployeeSearchTerm_ReturnsEmployeePage(string employeeSearchTerm)
        {
            // Arrange
            InitGetPage();
            BasePagingArgument employeePageArgument = new() { PageSize = 1, PageNumber = 1, SearchTerm = employeeSearchTerm };

            employeeRepository.GetPageAsync<EmployeeDto>(employeePageArgument).Returns(_employeePage);
            // Act
            BasePage<EmployeeDto> employeePage = await employeeService.GetPageAsync(employeePageArgument);

            // Assert
            // Trả về đúng định dạng
            Assert.That(_employeePage, Is.SameAs(employeePage));

            // Chỉ gọi 1 lần 
            _ = await employeeRepository.Received(1).GetPageAsync<EmployeeDto>(employeePageArgument);
        }
    }
}
