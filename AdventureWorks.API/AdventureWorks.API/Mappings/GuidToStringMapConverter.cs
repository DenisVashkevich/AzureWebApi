using System;
using AutoMapper;

namespace AdventureWorks.API.Mappings
{
    public class GuidToStringMapConverter: IValueConverter<Guid, string>
    {
        public string Convert(Guid sourceMember, ResolutionContext context)
        {
            var result = sourceMember.ToString();
            return result;
        }
    }
}
