//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:39
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
    /// Assembler for <see cref="portaluser"/> and <see cref="portaluserDto"/>.
    /// </summary>
    public static partial class portaluserAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="portaluserDto"/> converted from <see cref="portaluser"/>.</param>
        static partial void OnDTO(this portaluser entity, portaluserDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="portaluser"/> converted from <see cref="portaluserDto"/>.</param>
        static partial void OnEntity(this portaluserDto dto, portaluser entity);

        /// <summary>
        /// Converts this instance of <see cref="portaluserDto"/> to an instance of <see cref="portaluser"/>.
        /// </summary>
        /// <param name="dto"><see cref="portaluserDto"/> to convert.</param>
        public static portaluser ToEntity(this portaluserDto dto)
        {
            if (dto == null) return null;

            var entity = new portaluser();

            entity.PortalUserId = dto.PortalUserId;
            entity.UserName = dto.UserName;
            entity.Password = dto.Password;
            entity.IsDeleted = dto.IsDeleted;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="portaluser"/> to an instance of <see cref="portaluserDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="portaluser"/> to convert.</param>
        public static portaluserDto ToDTO(this portaluser entity)
        {
            if (entity == null) return null;

            var dto = new portaluserDto();

            dto.PortalUserId = entity.PortalUserId;
            dto.UserName = entity.UserName;
            dto.Password = entity.Password;
            dto.IsDeleted = entity.IsDeleted;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="portaluserDto"/> to an instance of <see cref="portaluser"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<portaluser> ToEntities(this IEnumerable<portaluserDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="portaluser"/> to an instance of <see cref="portaluserDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<portaluserDto> ToDTOs(this IEnumerable<portaluser> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
