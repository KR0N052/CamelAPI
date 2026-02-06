using CamelAPI;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CamelAPI.Tests
{
    public class CamelTests 
    { 
        private CamelDb CreateDb() 
        { 
            var options = new DbContextOptionsBuilder<CamelDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options; 
            return new CamelDb(options); 
        } 

        [Fact] 
        public void HumpCount_InvalidValue_ShouldThrow() 
        { 
            var camel = new Camel { Name = "Test" }; 

            Assert.Throws<ArgumentOutOfRangeException>(() => 
            { 
                camel.HumpCount = 3; 
            }); 
        }

        [Fact]
        public async Task AddCamel_ShouldPersist()
        {
            var db = CreateDb(); 
            var camel = new Camel 
            { 
                Name = "Camel1", 
                HumpCount = 2 
            }; 

            db.Camels.Add(camel); 

            await db.SaveChangesAsync(); 

            Assert.Equal(1, db.Camels.Count());
        }

        [Fact] 
        public async Task GetCamelById_ShouldReturnCamel() 
        { 
            var db = CreateDb(); 

            var camel = new Camel{ Name = "Camel2", HumpCount = 1 }; 
            db.Camels.Add(camel); 
            await db.SaveChangesAsync(); 

            var result = await db.Camels.FindAsync(camel.Id); 

            Assert.NotNull(result); 
            Assert.Equal("Camel2", result!.Name); 
        }

        [Fact] 
        public async Task DeleteCamel_ShouldRemove() 
        { 
            var db = CreateDb();

            var camel = new Camel { Name = "Camel3", HumpCount = 2 }; 
            db.Camels.Add(camel); 
            await db.SaveChangesAsync(); 

            db.Camels.Remove(camel); 
            await db.SaveChangesAsync(); 

            Assert.Empty(db.Camels); }
    }
}
