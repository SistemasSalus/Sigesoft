//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:24
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
    /// Assembler for <see cref="regcovid_get_nodesResult"/> and <see cref="regcovid_get_nodesResultDto"/>.
    /// </summary>
    public static partial class regcovid_get_nodesResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_nodesResultDto"/> converted from <see cref="regcovid_get_nodesResult"/>.</param>
        static partial void OnDTO(this regcovid_get_nodesResult entity, regcovid_get_nodesResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_nodesResult"/> converted from <see cref="regcovid_get_nodesResultDto"/>.</param>
        static partial void OnEntity(this regcovid_get_nodesResultDto dto, regcovid_get_nodesResult entity);

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_nodesResultDto"/> to an instance of <see cref="regcovid_get_nodesResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_nodesResultDto"/> to convert.</param>
        public static regcovid_get_nodesResult ToEntity(this regcovid_get_nodesResultDto dto)
        {
            if (dto == null) return null;

            var entity = new regcovid_get_nodesResult();

            entity.NodeId = dto.NodeId;
            entity.NodeName = dto.NodeName;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_nodesResult"/> to an instance of <see cref="regcovid_get_nodesResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_nodesResult"/> to convert.</param>
        public static regcovid_get_nodesResultDto ToDTO(this regcovid_get_nodesResult entity)
        {
            if (entity == null) return null;

            var dto = new regcovid_get_nodesResultDto();

            dto.NodeId = entity.NodeId;
            dto.NodeName = entity.NodeName;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_nodesResultDto"/> to an instance of <see cref="regcovid_get_nodesResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<regcovid_get_nodesResult> ToEntities(this IEnumerable<regcovid_get_nodesResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_nodesResult"/> to an instance of <see cref="regcovid_get_nodesResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<regcovid_get_nodesResultDto> ToDTOs(this IEnumerable<regcovid_get_nodesResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
