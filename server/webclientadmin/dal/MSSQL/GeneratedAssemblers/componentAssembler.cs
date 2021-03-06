//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:16:04
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Server.WebClientAdmin.DAL;

namespace Sigesoft.Server.WebClientAdmin.BE
{

    /// <summary>
    /// Assembler for <see cref="component"/> and <see cref="componentDto"/>.
    /// </summary>
    public static partial class componentAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="componentDto"/> converted from <see cref="component"/>.</param>
        static partial void OnDTO(this component entity, componentDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="component"/> converted from <see cref="componentDto"/>.</param>
        static partial void OnEntity(this componentDto dto, component entity);

        /// <summary>
        /// Converts this instance of <see cref="componentDto"/> to an instance of <see cref="component"/>.
        /// </summary>
        /// <param name="dto"><see cref="componentDto"/> to convert.</param>
        public static component ToEntity(this componentDto dto)
        {
            if (dto == null) return null;

            var entity = new component();

            entity.v_ComponentId = dto.v_ComponentId;
            entity.v_Name = dto.v_Name;
            entity.i_CategoryId = dto.i_CategoryId;
            entity.r_CostPrice = dto.r_CostPrice;
            entity.r_BasePrice = dto.r_BasePrice;
            entity.r_SalePrice = dto.r_SalePrice;
            entity.i_DiagnosableId = dto.i_DiagnosableId;
            entity.i_IsApprovedId = dto.i_IsApprovedId;
            entity.i_ComponentTypeId = dto.i_ComponentTypeId;
            entity.i_UIIsVisibleId = dto.i_UIIsVisibleId;
            entity.i_UIIndex = dto.i_UIIndex;
            entity.i_ValidInDays = dto.i_ValidInDays;
            entity.i_GroupedComponentId = dto.i_GroupedComponentId;
            entity.i_IsDeleted = dto.i_IsDeleted;
            entity.i_InsertUserId = dto.i_InsertUserId;
            entity.d_InsertDate = dto.d_InsertDate;
            entity.i_UpdateUserId = dto.i_UpdateUserId;
            entity.d_UpdateDate = dto.d_UpdateDate;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="component"/> to an instance of <see cref="componentDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="component"/> to convert.</param>
        public static componentDto ToDTO(this component entity)
        {
            if (entity == null) return null;

            var dto = new componentDto();

            dto.v_ComponentId = entity.v_ComponentId;
            dto.v_Name = entity.v_Name;
            dto.i_CategoryId = entity.i_CategoryId;
            dto.r_CostPrice = entity.r_CostPrice;
            dto.r_BasePrice = entity.r_BasePrice;
            dto.r_SalePrice = entity.r_SalePrice;
            dto.i_DiagnosableId = entity.i_DiagnosableId;
            dto.i_IsApprovedId = entity.i_IsApprovedId;
            dto.i_ComponentTypeId = entity.i_ComponentTypeId;
            dto.i_UIIsVisibleId = entity.i_UIIsVisibleId;
            dto.i_UIIndex = entity.i_UIIndex;
            dto.i_ValidInDays = entity.i_ValidInDays;
            dto.i_GroupedComponentId = entity.i_GroupedComponentId;
            dto.i_IsDeleted = entity.i_IsDeleted;
            dto.i_InsertUserId = entity.i_InsertUserId;
            dto.d_InsertDate = entity.d_InsertDate;
            dto.i_UpdateUserId = entity.i_UpdateUserId;
            dto.d_UpdateDate = entity.d_UpdateDate;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="componentDto"/> to an instance of <see cref="component"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<component> ToEntities(this IEnumerable<componentDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="component"/> to an instance of <see cref="componentDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<componentDto> ToDTOs(this IEnumerable<component> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
