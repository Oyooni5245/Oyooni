using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Dtos.AvailableTimes;
using System.Collections.Generic;
using System.Linq;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// Extensions for business models mappings
    /// </summary>
    public static class BusinessModelsMappings
    {
        /// <summary>
        /// Maps a <see cref="AppUser"/> to a <see cref="AppUserDto"/>
        /// </summary>
        public static AppUserDto ToAppUserDto(this AppUser appUser) => new AppUserDto(appUser.FirstName, appUser.LastName);

        /// <summary>
        /// Maps an IEnumerable of <see cref="AvailableTime"/> to a Dictionary by grouping all times 
        /// by the day of week idenifier
        /// </summary>
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
