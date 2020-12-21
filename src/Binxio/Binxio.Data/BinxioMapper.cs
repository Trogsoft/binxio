using AutoMapper;
using Binxio.Abstractions;
using Binxio.Common.Clients;
using Binxio.Common.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Data
{
    public class BinxioMapper : IXioMapper
    {

        IMapper mapper;

        MapperConfiguration mpc = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Client, ClientModel>().ReverseMap();
            cfg.CreateMap<User, UserPublicInfoModel>()
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.FirstName + " " + z.LastName))
                .ReverseMap();

            cfg.CreateMap<User, UserModel>()
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.FirstName + " " + z.LastName))
                .ReverseMap();
        });

        public BinxioMapper()
        {
            mapper = mpc.CreateMapper();
        }

        public T Map<T>(object source) => mapper.Map<T>(source);

        public TDest Map<TSource, TDest>(TSource source) => mapper.Map<TSource, TDest>(source);
    }
}
