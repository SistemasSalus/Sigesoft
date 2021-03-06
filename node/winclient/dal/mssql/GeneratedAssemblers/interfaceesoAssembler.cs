//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:34
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
    /// Assembler for <see cref="interfaceeso"/> and <see cref="interfaceesoDto"/>.
    /// </summary>
    public static partial class interfaceesoAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="interfaceesoDto"/> converted from <see cref="interfaceeso"/>.</param>
        static partial void OnDTO(this interfaceeso entity, interfaceesoDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="interfaceeso"/> converted from <see cref="interfaceesoDto"/>.</param>
        static partial void OnEntity(this interfaceesoDto dto, interfaceeso entity);

        /// <summary>
        /// Converts this instance of <see cref="interfaceesoDto"/> to an instance of <see cref="interfaceeso"/>.
        /// </summary>
        /// <param name="dto"><see cref="interfaceesoDto"/> to convert.</param>
        public static interfaceeso ToEntity(this interfaceesoDto dto)
        {
            if (dto == null) return null;

            var entity = new interfaceeso();

            entity.i_InterfaceESOId = dto.i_InterfaceESOId;
            entity.v_ServiceId = dto.v_ServiceId;
            entity.v_ComponentFieldId = dto.v_ComponentFieldId;
            entity.v_ResultComponent = dto.v_ResultComponent;
            entity.i_SystemUserId = dto.i_SystemUserId;
            entity.i_ImportStatus = dto.i_ImportStatus;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="interfaceeso"/> to an instance of <see cref="interfaceesoDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="interfaceeso"/> to convert.</param>
        public static interfaceesoDto ToDTO(this interfaceeso entity)
        {
            if (entity == null) return null;

            var dto = new interfaceesoDto();

            dto.i_InterfaceESOId = entity.i_InterfaceESOId;
            dto.v_ServiceId = entity.v_ServiceId;
            dto.v_ComponentFieldId = entity.v_ComponentFieldId;
            dto.v_ResultComponent = entity.v_ResultComponent;
            dto.i_SystemUserId = entity.i_SystemUserId;
            dto.i_ImportStatus = entity.i_ImportStatus;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="interfaceesoDto"/> to an instance of <see cref="interfaceeso"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<interfaceeso> ToEntities(this IEnumerable<interfaceesoDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="interfaceeso"/> to an instance of <see cref="interfaceesoDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<interfaceesoDto> ToDTOs(this IEnumerable<interfaceeso> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
