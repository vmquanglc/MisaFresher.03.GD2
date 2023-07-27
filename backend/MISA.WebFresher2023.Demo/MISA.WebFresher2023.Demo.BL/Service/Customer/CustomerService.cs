using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;
using System.Reflection;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class CustomerService : BaseService<Customer, CustomerDto, CustomerCreateDto, CustomerUpdateDto>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAndGroupRepository _customerAndGroupRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IOtherLocationRepository _otherLocationRepository;
        private readonly IContactInforRepository _contactInforRepository;
        private readonly ISpecificAddressRepository _specificAddressRepository;
        public CustomerService(
            ICustomerRepository customerRepository,
            ICustomerAndGroupRepository customerAndGroupRepository,
            IBankAccountRepository bankAccountRepository,
            IOtherLocationRepository otherLocationRepository,
            IContactInforRepository contactInforRepository,
            IMSDatabase msDatabase,
            ISpecificAddressRepository specificAddressRepository,
            IMapper mapper) : base(msDatabase, customerRepository, mapper)
        {
            _customerRepository = customerRepository;
            _customerAndGroupRepository = customerAndGroupRepository;
            _bankAccountRepository = bankAccountRepository;
            _otherLocationRepository = otherLocationRepository;
            _contactInforRepository = contactInforRepository;
            _specificAddressRepository = specificAddressRepository;
        }

        /// <summary>
        /// Hàm lấy một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>CustomerDto</returns>
        /// <exception cref="BadRequestException"></exception>
        /// Author: LeDucTiep (14/07/2023)
        public override async Task<CustomerDto?> GetAsync(Guid id)
        {
            try
            {
                // Kiểm tra lỗi
                if (id.Equals(Guid.Empty))
                {
                    List<int> errorList = new() { (int)RequestErrorCode.GuidInvalid };
                    throw new BadRequestException(errorList);
                }

                // Thực hiện
                Customer? customer = await _baseRepository.GetAsync(id);

                CustomerDto customerDto = _mapper.Map<CustomerDto>(customer);

                if (customer != null)
                {

                    customerDto.CustomerGroupIds = await _customerAndGroupRepository.GetCustomerGroupIdByCustomerIdAsync(id);

                    customerDto.BankAccounts = await _bankAccountRepository.GetByCustomerIdAsync(id);

                    var otherLocation = await _otherLocationRepository.GetByCustomerIdAsync(id);

                    var otherLocationDto = _mapper.Map<OtherLocationDto>(otherLocation);

                    customerDto.OtherLocation = otherLocationDto;

                    if (customer.ContactInforId != null)
                    {
                        var contactInfor = await _contactInforRepository.GetAsync((Guid)customer.ContactInforId);

                        var contactInforDto = _mapper.Map<ContactInforDto>(contactInfor);

                        customerDto.ContactInfor = contactInforDto;
                    }

                    if (otherLocation != null)
                    {
                        List<SpecificAddress> specificAddress = await _specificAddressRepository.GetByOtherLocationIdAsync(otherLocation.OtherLocationId);

                        customerDto.OtherLocation.SpecificAddresses = specificAddress.ToArray();
                    }
                }

                return customerDto;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã code mới</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<string> GetNewCustomerCodeAsync()
        {
            try
            {
                return await _customerRepository.GetNewCustomerCodeAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm thêm mới một bản ghi
        /// </summary>
        /// <param name="customerCreateDto">Thông tin cần thêm</param>
        /// <returns>Thông tin bản ghi đã thêm</returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="InternalException"></exception>
        /// Author: LeDucTiep (14/07/2023)
        public override async Task<CustomerDto> PostAsync(CustomerCreateDto customerCreateDto)
        {
            _msDatabase.BeginTransaction();

            try
            {
                CustomerDto customerDto = _mapper.Map<CustomerDto>(customerCreateDto);

                List<int> errorCodes = new();

                errorCodes.AddRange(base.Validate(customerDto));

                try
                {
                    List<int> ints = await base.PostValidate(customerDto);

                    errorCodes.AddRange(ints);
                }
                catch { }


                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);


                // Lấy customer
                Guid customerId = Guid.NewGuid();
                Guid contactInforId = Guid.NewGuid();
                Guid termsOfPaymentId = Guid.NewGuid();

                Customer customer = _mapper.Map<Customer>(customerCreateDto);

                List<CustomerAndGroup> customerAndGroups = new();

                List<BankAccount> bankAccounts = new();

                List<SpecificAddress> specificAddresses = new();

                OtherLocation otherLocation = null;

                ContactInfor contactInfor = null;


                if (customerCreateDto.CustomerGroupIds != null)
                {
                    foreach (Guid item in customerCreateDto.CustomerGroupIds)
                    {
                        CustomerAndGroup customerAndGroup = new();

                        customerAndGroup.CustomerId = customerId;
                        customerAndGroup.CustomerGroupId = item;

                        customerAndGroups.Add(customerAndGroup);
                    }
                }

                if (customerCreateDto.BankAccounts != null)
                {
                    foreach (BankAccount item in customerCreateDto.BankAccounts)
                    {
                        item.CustomerId = customerId;
                        if (!string.IsNullOrEmpty(item.BankAccountNumber))
                        {
                            bankAccounts.Add(item);
                        }
                    }
                }

                if (customerCreateDto.OtherLocation != null)
                {
                    otherLocation = _mapper.Map<OtherLocation>(customerCreateDto.OtherLocation);
                    otherLocation.CustomerId = customerId;

                    if (customerCreateDto.OtherLocation.SpecificAddresses != null && customerCreateDto.OtherLocation.SpecificAddresses.Length > 0)
                    {
                        specificAddresses = customerCreateDto.OtherLocation.SpecificAddresses.ToList();
                    }
                }

                if (customerCreateDto.ContactInfor != null)
                {
                    customer.ContactInforId = contactInforId;
                    contactInfor = _mapper.Map<ContactInfor>(customerCreateDto.ContactInfor);
                }

                // Ghi vao database
                if (contactInfor != null)
                    await _contactInforRepository.PostAsync(contactInfor, contactInforId);

                int changedCount = await _customerRepository.PostAsync(customer, customerId);

                if (otherLocation != null)
                {
                    await _otherLocationRepository.PostAsync(otherLocation, Guid.NewGuid());

                    if (specificAddresses.Count > 0)
                    {
                        foreach (var address in specificAddresses)
                        {
                            address.OtherLocationId = otherLocation.OtherLocationId;
                            await _specificAddressRepository.PostAsync(address, Guid.NewGuid());
                        }
                    }
                }



                foreach (CustomerAndGroup item in customerAndGroups)
                {
                    await _customerAndGroupRepository.PostAsync(item, Guid.Empty);
                }

                foreach (BankAccount item in bankAccounts)
                {
                    await _bankAccountRepository.PostAsync(item, Guid.NewGuid());
                }

                if (changedCount == 0)
                    throw new InternalException();

                customerDto = _mapper.Map<CustomerDto>(customer);

                _msDatabase.Commit();

                return customerDto;
            }
            catch (Exception ex)
            {
                _msDatabase.Rollback();
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm sửa một bản ghi
        /// </summary>
        /// <param name="id">Id của một bản ghi </param>
        /// <param name="customerUpdateDto">Bản ghi</param>
        /// <returns>Số dòng đã thay đổi </returns>
        /// <exception cref="BadRequestException">Lỗi</exception>
        /// Author: LeDucTiep (14/07/2023)
        public override async Task<int> UpdateAsync(Guid id, CustomerUpdateDto customerUpdateDto)
        {
            if (customerUpdateDto == null)
                throw new BadRequestException();

            _msDatabase.BeginTransaction();

            try
            {
                CustomerDto customerDto = _mapper.Map<CustomerDto>(customerUpdateDto);

                List<int> errorCodes = new();

                errorCodes.AddRange(base.Validate(customerDto));

                try
                {
                    List<int> ints = await base.UpdateValidate(id, customerDto);

                    errorCodes.AddRange(ints);
                }
                catch { }


                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                // Lấy customer
                Guid customerId = id;

                Guid contactInforId = Guid.NewGuid();
                if (customerUpdateDto.ContactInfor != null && !customerUpdateDto.ContactInfor.ContactInforId.Equals(Guid.Empty))
                {
                    contactInforId = customerUpdateDto.ContactInfor.ContactInforId;
                }

                Customer customer = _mapper.Map<Customer>(customerUpdateDto);

                List<CustomerAndGroup> customerAndGroups = new();

                List<BankAccount> bankAccounts = new();

                OtherLocation otherLocation = null;

                ContactInfor contactInfor = null;


                if (customerUpdateDto.CustomerGroupIds != null)
                {
                    foreach (Guid item in customerUpdateDto.CustomerGroupIds)
                    {
                        CustomerAndGroup customerAndGroup = new();

                        customerAndGroup.CustomerId = customerId;
                        customerAndGroup.CustomerGroupId = item;

                        customerAndGroups.Add(customerAndGroup);
                    }
                }

                if (customerUpdateDto.BankAccounts != null)
                {
                    foreach (BankAccount item in customerUpdateDto.BankAccounts)
                    {
                        item.CustomerId = customerId;
                        if (!string.IsNullOrEmpty(item.BankAccountNumber))
                        {
                            bankAccounts.Add(item);
                        }
                    }
                }

                List<SpecificAddress>? SpecificAddresses = null;

                if (customerUpdateDto.OtherLocation != null)
                {
                    otherLocation = _mapper.Map<OtherLocation>(customerUpdateDto.OtherLocation);
                    otherLocation.CustomerId = customerId;

                    if (customerUpdateDto.OtherLocation.SpecificAddresses != null)
                    {
                        SpecificAddresses = customerUpdateDto.OtherLocation.SpecificAddresses.ToList();
                    }
                }

                if (customerUpdateDto.ContactInfor != null)
                {
                    customer.ContactInforId = contactInforId;
                    contactInfor = _mapper.Map<ContactInfor>(customerUpdateDto.ContactInfor);
                }

                // Ghi vao database
                if (otherLocation != null)
                    if (!otherLocation.OtherLocationId.Equals(Guid.Empty) && await _otherLocationRepository.CheckExistedAsync(otherLocation.OtherLocationId))
                        await _otherLocationRepository.UpdateAsync(otherLocation.OtherLocationId, otherLocation);
                    else
                        await _otherLocationRepository.PostAsync(otherLocation, Guid.NewGuid());

                if (contactInfor != null)
                    if (await _contactInforRepository.CheckExistedAsync(contactInforId))
                        await _contactInforRepository.UpdateAsync(contactInforId, contactInfor);
                    else
                        await _contactInforRepository.PostAsync(contactInfor, contactInforId);

                int changedCount = 0;

                if (await _customerRepository.CheckExistedAsync(customerId))
                    await _customerRepository.UpdateAsync(customerId, customer);
                else
                    changedCount = await _customerRepository.PostAsync(customer, customerId);


                if (customerAndGroups.Count > 0)
                {
                    string customerGroupList = "'";
                    foreach (CustomerAndGroup item in customerAndGroups)
                    {
                        customerGroupList += item.CustomerGroupId + "','";
                        if (!await _customerAndGroupRepository.CheckExistedAsync(item.CustomerGroupId, item.CustomerId))
                            await _customerAndGroupRepository.PostAsync(item, Guid.Empty);
                    }

                    customerGroupList = customerGroupList[..^2];
                    await _customerAndGroupRepository.DeleteNotInAsync(customerGroupList, customerId);
                }

                if (bankAccounts.Count > 0)
                {
                    string bankAccountNumberList = "'";
                    foreach (BankAccount item in bankAccounts)
                    {
                        bankAccountNumberList += item.BankAccountNumber + "','";
                        if (await _bankAccountRepository.CheckExistedAsync(item.BankAccountId))
                            await _bankAccountRepository.UpdateAsync(item.BankAccountId, item);
                        else
                            await _bankAccountRepository.PostAsync(item, Guid.NewGuid());
                    }

                    bankAccountNumberList = bankAccountNumberList[..^2];
                    await _bankAccountRepository.DeleteNotInAsync(bankAccountNumberList, customerId);
                }

                if (SpecificAddresses != null && SpecificAddresses.Count > 0)
                {
                    string addressList = "'";
                    foreach (SpecificAddress item in SpecificAddresses)
                    {
                        item.OtherLocationId = otherLocation.OtherLocationId;

                        addressList += item.Address + "','";
                        if (item.SpecificAddressId != null && await _specificAddressRepository.CheckExistedAsync((Guid)item.SpecificAddressId))
                            await _specificAddressRepository.UpdateAsync((Guid)item.SpecificAddressId, item);
                        else
                            await _specificAddressRepository.PostAsync(item, Guid.NewGuid());
                    }

                    addressList = addressList[..^2];
                    await _specificAddressRepository.DeleteNotInAsync(addressList, otherLocation.OtherLocationId);
                }

                _msDatabase.Commit();

                return changedCount;
            }
            catch (Exception ex)
            {
                _msDatabase.Rollback();
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// hàm kiểm tra code bị trùng
        /// </summary>
        /// <param name="code">Mã khách hàng</param>
        /// <returns>bool</returns>
        /// <exception cref="BadRequestException">Lỗi validate</exception>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckCodeDuplicatedAsync(string code)
        {
            try
            {
                PropertyInfo? property = typeof(CustomerDto).GetProperty("CustomerCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && code != null)
                    {
                        int valueLength = code.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)CustomerErrorCode.CustomerCodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _customerRepository.CheckExistedCodeAsync(code);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm mã khách hàng đang sửa có bị trùng không 
        /// </summary>
        /// <param name="customerCode">Mã khách hàng đang sửa</param>
        /// <param name="itsCode">Mã khách hàng trước khi sửa</param>
        /// <returns>bool</returns>
        /// <exception cref="BadRequestException">Lỗi validate</exception>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string customerCode, string itsCode)
        {
            try
            {
                PropertyInfo? property = typeof(CustomerDto).GetProperty("CustomerCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && customerCode != null)
                    {
                        int valueLength = customerCode.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)CustomerErrorCode.CustomerCodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _customerRepository.CheckEditCodeDuplicatedAsync(customerCode, itsCode);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }
    }
}
