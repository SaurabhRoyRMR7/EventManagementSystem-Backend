using AutoMapper;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.AutoMapper
{
    public class EventMappingProfile :Profile
    {
        public EventMappingProfile()
        {
            
            CreateMap<CreateEventDTO, Event>()
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
           .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => "System"))
           .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => false));

            CreateMap<EventRegistrationDTO, EventRegistration>()
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
          .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => "System"))
          .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate ?? DateTime.Now));

            CreateMap<EventRegistrationFormsFieldDTO, EventRegistrationFormsField>()
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
           .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => "System"));

            //CreateMap<FieldOptionDTO, FieldOption>()
            //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            //.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));

            CreateMap<User, UserDTO>();
            CreateMap<EventRegistration, EventRegistrationDTO>();
            CreateMap<Event, EventDTO>();


        }
    }
}
