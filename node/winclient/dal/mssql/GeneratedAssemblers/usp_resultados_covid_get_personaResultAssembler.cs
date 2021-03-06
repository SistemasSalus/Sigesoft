//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:27
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
    /// Assembler for <see cref="usp_resultados_covid_get_personaResult"/> and <see cref="usp_resultados_covid_get_personaResultDto"/>.
    /// </summary>
    public static partial class usp_resultados_covid_get_personaResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="usp_resultados_covid_get_personaResultDto"/> converted from <see cref="usp_resultados_covid_get_personaResult"/>.</param>
        static partial void OnDTO(this usp_resultados_covid_get_personaResult entity, usp_resultados_covid_get_personaResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="usp_resultados_covid_get_personaResult"/> converted from <see cref="usp_resultados_covid_get_personaResultDto"/>.</param>
        static partial void OnEntity(this usp_resultados_covid_get_personaResultDto dto, usp_resultados_covid_get_personaResult entity);

        /// <summary>
        /// Converts this instance of <see cref="usp_resultados_covid_get_personaResultDto"/> to an instance of <see cref="usp_resultados_covid_get_personaResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="usp_resultados_covid_get_personaResultDto"/> to convert.</param>
        public static usp_resultados_covid_get_personaResult ToEntity(this usp_resultados_covid_get_personaResultDto dto)
        {
            if (dto == null) return null;

            var entity = new usp_resultados_covid_get_personaResult();

            entity.v_PersonId = dto.v_PersonId;
            entity.v_Name = dto.v_Name;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="usp_resultados_covid_get_personaResult"/> to an instance of <see cref="usp_resultados_covid_get_personaResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="usp_resultados_covid_get_personaResult"/> to convert.</param>
        public static usp_resultados_covid_get_personaResultDto ToDTO(this usp_resultados_covid_get_personaResult entity)
        {
            if (entity == null) return null;

            var dto = new usp_resultados_covid_get_personaResultDto();

            dto.v_PersonId = entity.v_PersonId;
            dto.v_Name = entity.v_Name;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_resultados_covid_get_personaResultDto"/> to an instance of <see cref="usp_resultados_covid_get_personaResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<usp_resultados_covid_get_personaResult> ToEntities(this IEnumerable<usp_resultados_covid_get_personaResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_resultados_covid_get_personaResult"/> to an instance of <see cref="usp_resultados_covid_get_personaResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<usp_resultados_covid_get_personaResultDto> ToDTOs(this IEnumerable<usp_resultados_covid_get_personaResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
