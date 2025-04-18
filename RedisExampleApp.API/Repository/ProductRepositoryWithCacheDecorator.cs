using RedisExampleApp.API.Models;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {

        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        //private readonly RedisSevice _redisService;
        private readonly IDatabase _cacheRepository;
        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository /*RedisService redisService*/)
        {
            _productRepository = productRepository;
            //_redisService = redisService;
            //_cacheRepository = _redisService.GetDb(2);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            var products = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product)); // cache'e ekleyecek
            }
            return products;

        }

        public async Task<List<Product>> GetAsync() // GetAsync methodu cache'den datayı alacak ve eğer cache'de yoksa db'den alıp cache'e atacak
        {
            if(!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();
            
            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);

                products.Add(product);

            }
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey,id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(p => p.Id == id); // cache'den datayı alacak ve eğer cache'de yoksa db'den alıp cache'e atacak
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();
            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey,p.Id,JsonSerializer.Serialize(p)); // redisten bu id ye göre datayı çekicek db den datayı cache'leyecek
            });
            return products;
        }

    }
}
