using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using SmartRefund.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Tests.ApplicationTests
{
    public class CacheServiceTests
    {
        private readonly CacheService _cacheService;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CacheService> _logger;

        public CacheServiceTests()
        {
            _memoryCache = Substitute.For<IMemoryCache>();
            _configuration = Substitute.For<IConfiguration>();
            _logger = Substitute.For<ILogger<CacheService>>();
            _cacheService = new CacheService(_memoryCache, _configuration, _logger);
        }


        [Fact]
        public async Task GetCachedDataAsync_Should_Return_Empty_List_When_Data_Not_In_Cache()
        {
            // Arrange
            var key = "testKey";

            // Act
            var result = await _cacheService.GetCachedDataAsync<string>(key);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SetCachedDataAsync_Should_Add_Data_To_Cache()
        {
            // Arrange
            var key = "testKey";
            var data = new List<string> { "data1", "data2", "data3" };
            _configuration.GetSection("CacheSettings")["SlidingExpirationSeconds"].Returns("300");
            _configuration.GetSection("CacheSettings")["AbsoluteExpirationSeconds"].Returns("600");
            _configuration.GetSection("CacheSettings")["MaxMemorySizeMB"].Returns("50");

            // Act
            await _cacheService.SetCachedDataAsync(key, data);
            _memoryCache.TryGetValue<IEnumerable<string>>(key, out var cachedData)
                .Returns(callInfo =>
                {
                    callInfo[1] = data;
                    return true;
                });

            // Assert
            var result = await _cacheService.GetCachedDataAsync<string>(key);
            result.Should().BeEquivalentTo(data);
        }
    }
}

