using AutoMapper;
using Binxio.Common.Clients;
using Binxio.Common.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class Mapper
    {
        public IMapper mapper;

        public MapperConfiguration mpc = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Data.Project, ProjectModel>();
            cfg.CreateMap<Data.Client, ClientModel>();
        });

        public Mapper()
        {
            mapper = mpc.CreateMapper();
        }

        public T Map<T>(object source) => mapper.Map<T>(source);

        public TDest Map<TSource, TDest>(TSource source) => mapper.Map<TSource, TDest>(source);

    }
}
