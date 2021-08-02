//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:26
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
    /// Assembler for <see cref="usp_get_valorescomponenteResult"/> and <see cref="usp_get_valorescomponenteResultDto"/>.
    /// </summary>
    public static partial class usp_get_valorescomponenteResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="usp_get_valorescomponenteResultDto"/> converted from <see cref="usp_get_valorescomponenteResult"/>.</param>
        static partial void OnDTO(this usp_get_valorescomponenteResult entity, usp_get_valorescomponenteResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="usp_get_valorescomponenteResult"/> converted from <see cref="usp_get_valorescomponenteResultDto"/>.</param>
        static partial void OnEntity(this usp_get_valorescomponenteResultDto dto, usp_get_valorescomponenteResult entity);

        /// <summary>
        /// Converts this instance of <see cref="usp_get_valorescomponenteResultDto"/> to an instance of <see cref="usp_get_valorescomponenteResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="usp_get_valorescomponenteResultDto"/> to convert.</param>
        public static usp_get_valorescomponenteResult ToEntity(this usp_get_valorescomponenteResultDto dto)
        {
            if (dto == null) return null;

            var entity = new usp_get_valorescomponenteResult();

            entity.ComponentFieldId = dto.ComponentFieldId;
            entity.v_TextLabel = dto.v_TextLabel;
            entity.ServiceComponentFieldsId = dto.ServiceComponentFieldsId;
            entity.Value1 = dto.Value1;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="usp_get_valorescomponenteResult"/> to an instance of <see cref="usp_get_valorescomponenteResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="usp_get_valorescomponenteResult"/> to convert.</param>
        public static usp_get_valorescomponenteResultDto ToDTO(this usp_get_valorescomponenteResult entity)
        {
            if (entity == null) return null;

            var dto = new usp_get_valorescomponenteResultDto();

            dto.ComponentFieldId = entity.ComponentFieldId;
            dto.v_TextLabel = entity.v_TextLabel;
            dto.ServiceComponentFieldsId = entity.ServiceComponentFieldsId;
            dto.Value1 = entity.Value1;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_get_valorescomponenteResultDto"/> to an instance of <see cref="usp_get_valorescomponenteResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<usp_get_valorescomponenteResult> ToEntities(this IEnumerable<usp_get_valorescomponenteResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_get_valorescomponenteResult"/> to an instance of <see cref="usp_get_valorescomponenteResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<usp_get_valorescomponenteResultDto> ToDTOs(this IEnumerable<usp_get_valorescomponenteResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}