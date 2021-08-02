//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:30
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Node.WinClient.DAL;

namespace Sigesoft.Node.WinClient.BE
{

    /// <summary>
    /// Assembler for <see cref="campaniascovid"/> and <see cref="campaniascovidDto"/>.
    /// </summary>
    public static partial class campaniascovidAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="campaniascovidDto"/> converted from <see cref="campaniascovid"/>.</param>
        static partial void OnDTO(this campaniascovid entity, campaniascovidDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="campaniascovid"/> converted from <see cref="campaniascovidDto"/>.</param>
        static partial void OnEntity(this campaniascovidDto dto, campaniascovid entity);

        /// <summary>
        /// Converts this instance of <see cref="campaniascovidDto"/> to an instance of <see cref="campaniascovid"/>.
        /// </summary>
        /// <param name="dto"><see cref="campaniascovidDto"/> to convert.</param>
        public static campaniascovid ToEntity(this campaniascovidDto dto)
        {
            if (dto == null) return null;

            var entity = new campaniascovid();

            entity.id = dto.id;
            entity.Sede = dto.Sede;
            entity.Area = dto.Area;
            entity.DNI = dto.DNI;
            entity.ApellidoMaterno = dto.ApellidoMaterno;
            entity.ApellidoPaterno = dto.ApellidoPaterno;
            entity.Nombres = dto.Nombres;
            entity.Fecha = dto.Fecha;
            entity.Resultado = dto.Resultado;
            entity.Campa�a = dto.Campa�a;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="campaniascovid"/> to an instance of <see cref="campaniascovidDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="campaniascovid"/> to convert.</param>
        public static campaniascovidDto ToDTO(this campaniascovid entity)
        {
            if (entity == null) return null;

            var dto = new campaniascovidDto();

            dto.id = entity.id;
            dto.Sede = entity.Sede;
            dto.Area = entity.Area;
            dto.DNI = entity.DNI;
            dto.ApellidoMaterno = entity.ApellidoMaterno;
            dto.ApellidoPaterno = entity.ApellidoPaterno;
            dto.Nombres = entity.Nombres;
            dto.Fecha = entity.Fecha;
            dto.Resultado = entity.Resultado;
            dto.Campa�a = entity.Campa�a;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="campaniascovidDto"/> to an instance of <see cref="campaniascovid"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<campaniascovid> ToEntities(this IEnumerable<campaniascovidDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="campaniascovid"/> to an instance of <see cref="campaniascovidDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<campaniascovidDto> ToDTOs(this IEnumerable<campaniascovid> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}