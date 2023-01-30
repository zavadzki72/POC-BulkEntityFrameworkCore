using Bogus;
using Bulk.App.Models.Entities;

namespace Bulk.App
{
    public static class Utils
    {
        public static readonly List<StadiumEntity> _stadiumsToDev = new()
        {
            new StadiumEntity
            {
                Name = "Cicero pompeu de toledo",
                Nickname = "Morumbi",
                Capacity = 60000
            },
            new StadiumEntity
            {
                Name = "Alianz Parque",
                Nickname = "Palestra italia",
                Capacity = 40000
            },
        };

        public static readonly List<TeamEntity> _teamsToDev = new()
        {
            new TeamEntity
            {
                Name = "São Paulo FC",
                Initials = "SPFC",
                Country = "Brasil",
                Founded_At = DateTime.Now,
                StadiumId = _stadiumsToDev.First(x => x.Nickname.Equals("Morumbi")).Id,
                Stadium = _stadiumsToDev.First(x => x.Nickname.Equals("Morumbi"))
            },
            new TeamEntity
            {
                Name = "Sociedade esportiva Palmeiras",
                Initials = "SEP",
                Country = "Brasil",
                Founded_At = DateTime.Now,
                StadiumId = _stadiumsToDev.First(x => x.Nickname.Equals("Palestra italia")).Id,
                Stadium = _stadiumsToDev.First(x => x.Nickname.Equals("Palestra italia"))
            }
        };

        public static List<TeamEntity> GetTeamListRandom(int quantity, bool withId)
        {
            List<TeamEntity> teamList = new();
            var faker = new Faker("pt_BR");

            var stadiums = GetStadiumListRandom(quantity, withId);

            for (int i = 0; i < quantity; i++)
            {
                var team = new TeamEntity
                {
                    Name = Guid.NewGuid().ToString(),
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
                    Name = Guid.NewGuid().ToString(),
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
