using api.Controllers;
using api.DTOs;
using api.Helpers;
using api.Models;
using api.Repositories;
using api.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace api_test;

public class InsuranceControllerUnitTest
{

    private InsuranceRepository? repository;
    private InsuranceService? service;
    private IMapper mapper;
    public static DbContextOptions<InsuranceDBContext>? dbContextOptions { get; }
    public static string connectionString = "DataSource=Database\\app.db";

    static InsuranceControllerUnitTest()
    {
        dbContextOptions = new DbContextOptionsBuilder<InsuranceDBContext>()
            .UseSqlite(connectionString)
            .Options;
    }

    public InsuranceControllerUnitTest()
    {
        var context = new InsuranceDBContext(dbContextOptions!);
        var db = new DummyDataDBInitializer();
        db.Seed(context);

        repository = new InsuranceRepository(context);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        mapper = config.CreateMapper();

        service = new InsuranceService(repository, mapper);

    }

    #region Get By Id  

    [Fact]
    public async void Task_GetById_Return_OkResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var Id = 2;

        //Act  
        var data = await controller.GetById(Id);

        //Assert  
        Assert.IsType<OkObjectResult>(data);
    }

    [Fact]
    public async void Task_GetById_Return_NotFoundResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 200;

        //Act  
        var data = await controller.GetById(id);

        //Assert  
        Assert.IsType<NotFoundObjectResult>(data);
    }

    [Fact]
    public async void Task_GetById_MatchResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        int id = 1;

        //Act  
        var data = await controller.GetById(id);

        //Assert  
        Assert.IsType<OkObjectResult>(data);

        var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

        var insurance = okResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        Assert.Equal("Angloamericana de Seguros, S. A.", insurance.Data!.Name);
        Assert.Equal(0.10, insurance.Data.Fee);
    }

    #endregion

    #region Get All  

    [Fact]
    public async void Task_GetAllInsures_Return_OkResult()
    {
        //Arrange
        var filter = new PaginationFilter();
        var controller = new InsuranceController(service!);

        //Act  
        var data = await controller.GetAll(filter);

        //Assert  
        Assert.IsType<OkObjectResult>(data);
    }

    [Fact]
    public async void Task_GetAllInsures_MatchResult()
    {
        //Arrange
        var filter = new PaginationFilter();
        var controller = new InsuranceController(service!);

        //Act  
        var data = await controller.GetAll(filter);

        //Assert  
        Assert.IsType<OkObjectResult>(data);

        var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
        var insurance = okResult.Value.Should().BeAssignableTo<ResponseBase<List<Insurance>>>().Subject;

        Assert.Equal("Angloamericana de Seguros, S. A.", insurance.Data![0].Name);
        Assert.Equal(0.10, insurance.Data[0].Fee);

        Assert.Equal("Aseguradora Agropecuaria Dominicana, S.A.", insurance.Data![1].Name);
        Assert.Equal(0.10, insurance.Data[1].Fee);
    }

    #endregion

    #region Add New Insurance  

    [Fact]
    public async void Task_Add_ValidData_Return_OkResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var insurance = new InsuranceCreationDTO() { Name = "Test 1", Fee = 0.10, Status = true };

        //Act  
        var data = await controller.Create(insurance);

        //Assert  
        Assert.IsType<CreatedAtRouteResult>(data);
    }

    [Fact]
    public async void Task_Add_InvalidData_Return_BadRequest()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var insurance = new InsuranceCreationDTO() { Name = "Test tiene mas de 45 caracteres... Nombre muy largo", Fee = 0.10, Status = true };

        //Act              
        var data = await controller.Create(insurance);

        //Assert  
        Assert.IsType<BadRequestObjectResult>(data);
    }

    [Fact]
    public async void Task_Add_InvalidName_Return_BadRequest()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var insurance = new InsuranceCreationDTO() { Name = "Test tiene mas de 45 caracteres... Nombre muy largo", Fee = 0.10, Status = true };

        //Act              
        var data = await controller.Create(insurance);

        //Assert  
        //Assert  
        Assert.IsType<BadRequestObjectResult>(data);

        var badResult = data.Should().BeOfType<BadRequestObjectResult>().Subject;

        var result = badResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        Assert.Equal("El nombre no puede ser mayor de 45 caracteres.", result.Error);
    }

    [Fact]
    public async void Task_Add_InvalidFee_Return_BadRequest()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var insurance = new InsuranceCreationDTO() { Name = "Test 1", Fee = 0.45, Status = true };

        //Act              
        var data = await controller.Create(insurance);

        //Assert  
        Assert.IsType<BadRequestObjectResult>(data);

        var badResult = data.Should().BeOfType<BadRequestObjectResult>().Subject;

        var result = badResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        Assert.Equal("La aseguradora no puede cobrar menos del 0% ni mas del 25% de comisión", result.Error);

    }

    [Fact]
    public async void Task_Add_ValidData_MatchResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var insurance = new InsuranceCreationDTO() { Name = "Test 1", Fee = 0.15, Status = true };

        //Act              
        var data = await controller.Create(insurance);

        //Assert  
        Assert.IsType<CreatedAtRouteResult>(data);

        var okResult = data.Should().BeOfType<CreatedAtRouteResult>().Subject;

        var result = okResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        Assert.Equal("Test 1", result.Data!.Name);

    }

    #endregion

    #region Update Existing Insurance  

    [Fact]
    public async void Task_Update_ValidData_Return_OkResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 1;

        //Act  
        var existingInsurance = await controller.GetById(id);
        var okResult = existingInsurance.Should().BeOfType<OkObjectResult>().Subject;
        var result = okResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        var insurance = new InsuranceCreationDTO();
        insurance.Name = "Test 1";
        insurance.Fee = 0.05;
        insurance.Status = false;

        var updatedData = await controller.Update(id, insurance);

        //Assert  
        Assert.IsType<NoContentResult>(updatedData);
    }

    [Fact]
    public async void Task_Update_InvalidData_Return_BadRequest()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 1;

        //Act  
        var existingInsurance = await controller.GetById(id);
        var okResult = existingInsurance.Should().BeOfType<OkObjectResult>().Subject;
        var result = okResult.Value.Should().BeAssignableTo<ResponseBase<Insurance>>().Subject;

        var insurance = new InsuranceCreationDTO();
        insurance.Name = "Test 1 este es un nombre extremadamente largo para que de error";
        insurance.Fee = 0.05;
        insurance.Status = false;

        var updatedData = await controller.Update(id, insurance);

        //Assert  
        Assert.IsType<BadRequestObjectResult>(updatedData);
    }

    [Fact]
    public async void Task_Update_InvalidData_Return_NotFound()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 100;

        
        var insurance = new InsuranceCreationDTO();
        insurance.Name = "Test 1";
        insurance.Fee = 0.05;
        insurance.Status = false;

        var updatedData = await controller.Update(id, insurance);

        //Assert  
        Assert.IsType<NotFoundObjectResult>(updatedData);
    }

    #endregion

    #region Delete Insurance  

    [Fact]
    public async void Task_Delete_Insurance_Return_OkResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 1;

        //Act  
        var data = await controller.Delete(id);

        //Assert  
        Assert.IsType<NoContentResult>(data);
    }

    [Fact]
    public async void Task_Delete_Insurance_Return_NotFoundResult()
    {
        //Arrange  
        var controller = new InsuranceController(service!);
        var id = 100;

        //Act  
        var data = await controller.Delete(id);

        //Assert  
        Assert.IsType<NotFoundObjectResult>(data);
    }

    #endregion
}
