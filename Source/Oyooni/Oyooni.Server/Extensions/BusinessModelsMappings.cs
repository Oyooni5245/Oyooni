using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Dtos.AvailableTimes;
using System.Collections.Generic;
using System.Linq;

namespace Oyooni.Server.Extensions
{
    public static class BusinessModelsMappings
    {
        public static AppUserDto ToAppUserDto(this AppUser appUser) => new AppUserDto(appUser.FirstName, appUser.LastName);

        public static IDictionary<int, List<AvailableTimeDto>> ToGroupedAvailableTimeDto(this IEnumerable<AvailableTime> times)
        {
            return times.Select(t => new AvailableTimeDto
            {
                Id = t.Id,
                From = t.From.ToString(@"hh\:mm"),
                To = t.To.ToString(@"hh\:mm")
            }).GroupBy(t => t.DayOfWeekId).ToDictionary(group => group.Key, group => group.ToList());
        }
    }
}
