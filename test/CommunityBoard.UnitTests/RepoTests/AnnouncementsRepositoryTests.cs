using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CommunityBoard.UnitTests.RepoTests
{
    [Collection("Database collection")]
    public class AnnouncementsRepositoryTests : IClassFixture<DatabaseFixture>
    {
        protected DatabaseFixture _fixture;

        public AnnouncementsRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        public async Task FindALL_ReturnsNull_WhenDoesNotExist()
        {
            var announcements = await _fixture._repo.FindAllAsync();
            announcements.Should().BeNullOrEmpty();
            announcements.Count.Should().BeLessOrEqualTo(0);
        }

        //[Fact]
        //public async Task FindALL_ReturnsList_WhenDoesExist()
        //{
        //    //Arrange
        //    //_fixture.Seed();

        //    //Act
        //    var announcements = await _fixture._repo.FindAllAsync();
            
        //    //Assert
        //    announcements.Should().NotBeNullOrEmpty();
        //    announcements.Count.Should().BeGreaterOrEqualTo(1);
        //}
    }
}