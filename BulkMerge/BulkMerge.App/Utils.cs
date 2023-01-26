using Bogus;
using BulkMerge.App.Models.Entities;

namespace BulkMerge.App
{
    public static class Utils
    {
        public static List<TeamEntity> GetTeamListRandom(int quantity, bool withId)
        {
            List<TeamEntity> teamList = new();
            var faker = new Faker("pt_BR");

            var stadiums = GetStadiumListRandom(quantity, withId);

            for (int i = 0; i < quantity; i++)
            {
                var team = new TeamEntity
                {
                    Name = faker.Company.CompanyName(),
                    Initials = "INI",
                    Country = faker.Address.Country(),
                    Founded_At = DateTime.Now.AddYears(-(faker.Random.Int(1, 100))),
                    StadiumId = stadiums[i].Id,
                    Stadium = stadiums[i]
                };

                if (withId)
                    team.Id = i + 1;

                teamList.Add(team);
            }

            return teamList;
        }

        public static List<StadiumEntity> GetStadiumListRandom(int quantity, bool withId)
        {
            List<StadiumEntity> stadiumList = new();
            var faker = new Faker("pt_BR");

            for (int i = 0; i < quantity; i++)
            {
                var stadium = new StadiumEntity
                {
                    Name = faker.Company.CompanyName(),
                    Capacity = faker.Random.Int(1000, 500000),
                    Nickname = "APELIDO",
                };

                if (withId)
                    stadium.Id = i + 1;

                stadiumList.Add(stadium);
            }

            return stadiumList;
        }
    }
}
