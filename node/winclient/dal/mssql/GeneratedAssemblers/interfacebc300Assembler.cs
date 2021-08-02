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
    /// Assembler for <see cref="interfacebc300"/> and <see cref="interfacebc300Dto"/>.
    /// </summary>
    public static partial class interfacebc300Assembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="interfacebc300Dto"/> converted from <see cref="interfacebc300"/>.</param>
        static partial void OnDTO(this interfacebc300 entity, interfacebc300Dto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="interfacebc300"/> converted from <see cref="interfacebc300Dto"/>.</param>
        static partial void OnEntity(this interfacebc300Dto dto, interfacebc300 entity);

        /// <summary>
        /// Converts this instance of <see cref="interfacebc300Dto"/> to an instance of <see cref="interfacebc300"/>.
        /// </summary>
        /// <param name="dto"><see cref="interfacebc300Dto"/> to convert.</param>
        public static interfacebc300 ToEntity(this interfacebc300Dto dto)
        {
            if (dto == null) return null;

            var entity = new interfacebc300();

            entity.i_InterfaceBS300 = dto.i_InterfaceBS300;
            entity.v_ServiceId = dto.v_ServiceId;
            entity.v_ComponentId = dto.v_ComponentId;
            entity.v_ResultComponent = dto.v_ResultComponent;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="interfacebc300"/> to an instance of <see cref="interfacebc300Dto"/>.
        /// </summary>
        /// <param name="entity"><see cref="interfacebc300"/> to convert.</param>
        public static interfacebc300Dto ToDTO(this interfacebc300 entity)
        {
            if (entity == null) return null;

            var dto = new interfacebc300Dto();

            dto.i_InterfaceBS300 = entity.i_InterfaceBS300;
            dto.v_ServiceId = entity.v_ServiceId;
            dto.v_ComponentId = entity.v_ComponentId;
            dto.v_ResultComponent = entity.v_ResultComponent;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="interfacebc300Dto"/> to an instance of <see cref="interfacebc300"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<interfacebc300> ToEntities(this IEnumerable<interfacebc300Dto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="interfacebc300"/> to an instance of <see cref="interfacebc300Dto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<interfacebc300Dto> ToDTOs(this IEnumerable<interfacebc300> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
