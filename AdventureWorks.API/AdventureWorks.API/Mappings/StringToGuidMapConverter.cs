using System;
using System.Linq;
using AutoMapper;

namespace AdventureWorks.API.Mappings
{
    public class StringToGuidMapConverter: IValueConverter<string, Guid>
    {
        public Guid Convert(string sourceMember, ResolutionContext context)
        {
            return Guid.TryParse(sourceMember, out Guid result) ? result : new Guid(); ;
        }
    }
}
