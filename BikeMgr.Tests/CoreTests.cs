using BikeMgr.Core.Interface.Queries;
using BikeMgr.Core.Interface.Services;
using BikeMgr.Core.Models;
using BikeMgr.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BikeMgr.Tests
{
    [TestClass]
    public class CoreTests
    {
        private static Bike _bike;
        private static BikeType _bikeType;
        private static List<Bike> _bikeList;

        private static Mock<IBikeQueries> _mockQueries;
        private static Mock<IStorageService> _mockStorage;

        [TestInitialize]
        public void Initialize()
        {
            _bikeType = new BikeType()
            {
                ID = 1,
                TypeName = "Hybrid"
            };

            _bike = new Bike()
            {
                Name = "Flyer",
                ID = 1,
                Brand = "Test Brand",
                BikeTypeID = 1,
                FrameMaterial = "Steel",
                Price = 500,
                Wheels = 2,
                Image = new Blob(),
                ImageLocation = "DiskLocation",
                BikeType = _bikeType
            };
            _bikeList = new List<Bike>();
            _bikeList.Add(_bike);

            // mock bike queries
            _mockQueries = new Mock<IBikeQueries>();            
            _mockQueries.Setup(x => x.GetBikeTypeByID(It.IsAny<int>()))
                .Returns(_bikeType);
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>()))
                .Returns((Bike)null);
            _mockQueries.Setup(x => x.CreateBike(It.IsAny<Bike>()))
                .Returns(Task.FromResult(_bike));
            _mockQueries.Setup(x => x.DeleteBike(It.IsAny<Bike>()));
            _mockQueries.Setup(x => x.GetBikes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PageParams>()))
                .Returns(Task.FromResult(new Page<Bike>(_bikeList, 1, 1)));
            _mockQueries.Setup(x => x.UpdateBike(It.IsAny<Bike>()))
                .Returns(Task.FromResult(_bike));
            

            // mock bike storage service
            _mockStorage = new Mock<IStorageService>();
            _mockStorage.Setup(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>())).Returns(Task.FromResult(""));
            //_mockStorage.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            _mockStorage.Setup(x => x.DeleteFile(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task CreateBike_AddsOneBike()
        {
            //Arrange
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = await bikeService.CreateBike(_bike);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Bike));
            _mockQueries.Verify(x => x.CreateBike(It.IsAny<Bike>()), Times.Once);
            _mockStorage.Verify(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateBike_SavesWhenImageIsNull()
        {
            //Arrange
            Bike bike = _bike;
            bike.Image = null;
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = await bikeService.CreateBike(bike);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Bike));
            _mockQueries.Verify(x => x.CreateBike(It.IsAny<Bike>()), Times.Once);
            _mockStorage.Verify(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void DeleteBike_CallsDeleteOnce()
        {
            //Arrange
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(_bike);
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            bikeService.DeleteBike(1);

            //Assert
            _mockQueries.Verify(x => x.DeleteBike(It.IsAny<Bike>()), Times.Once);
            _mockStorage.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetBikeByID_ReturnsOneBike()
        {
            //Arrange
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(_bike);
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = bikeService.GetBikeByID(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Bike));
            _mockQueries.Verify(x => x.GetBikeByID(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void GetBikes_ReturnsPageOfBikes()
        {
            //Arrange
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = bikeService.GetBikes("", "", new PageParams { PageNo = 1, PageSize = 10 });

            //Assert
            Assert.IsInstanceOfType(result, typeof(Task<Page<Bike>>));
            _mockQueries.Verify(x => x.GetBikes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PageParams>()), Times.Once);
        }

        [TestMethod]
        public void GetBikeTypes_ReturnsArrayOfBikeType()
        {
            //Arrange
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = bikeService.GetBikeTypes();

            //Assert
            Assert.IsInstanceOfType(result, typeof(BikeType[]));
            _mockQueries.Verify(x => x.GetBikeTypes(), Times.Once);
        }

        [TestMethod]
        public void GetBikeTypeByID_ReturnsABikeType()
        {
            //Arrange
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = bikeService.GetBikeTypeByID(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BikeType));
            _mockQueries.Verify(x => x.GetBikeTypeByID(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateBike_ReturnsOneUpdatedBike()
        {
            //Arrange
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(_bike);
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = await bikeService.UpdateBike(1, _bike);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Bike));
            _mockQueries.Verify(x => x.UpdateBike(It.IsAny<Bike>()), Times.Once);
            _mockStorage.Verify(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _mockStorage.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateBike_UpdatesWhenImageIsNull()
        {
            //Arrange
            var bike = _bike;
            bike.Image = null;
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(bike);
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            var result = await bikeService.UpdateBike(1, bike);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Bike));
            _mockQueries.Verify(x => x.UpdateBike(It.IsAny<Bike>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateBike_ThrowsExceptionWhenSaveFileFails()
        {
            //Arrange
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(_bike);
            _mockStorage.Setup(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>())).Throws(new Exception());
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            try
            {
                var result = await bikeService.UpdateBike(1, _bike);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                _mockStorage.Verify(x => x.SaveFile(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            }
        }

        [TestMethod]
        public async Task UpdateBike_ThrowsExceptionWhenDeleteFileFails()
        {
            //Arrange
            _mockQueries.Setup(x => x.GetBikeByID(It.IsAny<int>())).Returns(_bike);
            _mockStorage.Setup(x => x.DeleteFile(It.IsAny<string>())).Throws(new Exception());
            var bikeService = new BikeService(_mockQueries.Object, _mockStorage.Object);

            //Act
            try
            {
                var result = await bikeService.UpdateBike(1, _bike);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                _mockQueries.Verify(x => x.UpdateBike(It.IsAny<Bike>()), Times.Once);
                _mockStorage.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);
            }
        }
    }
}
