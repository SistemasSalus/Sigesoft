using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportMusculoEsqueletico2
    {
        public string v_PersonId { get; set; }
        public string v_FullPersonName { get; set; }
        public string v_DocNumber { get; set; }
        public int? i_SexTypeId { get; set; }
        public string v_SexType { get; set; }
        public DateTime? d_BirthDate { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string Puesto { get; set; }

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaMedico { get; set; }
        public byte[] FirmaTecnologo { get; set; }
        public int i_AgePacient { get; set; }
        public byte[] b_Logo { get; set; }
        public string v_WorkingOrganizationName { get; set; }

        // Campos

        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_REPOSO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_PALPACION { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_FLEXION_40 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_EXTENSION_75 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_ROTACION_D_75 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_ROTACION_I_75 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_LATER_D_30_45 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_LATER_I_30_45 { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_LD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_A_COLUMNA_CERVICAL_DOLOR_MOVIMIENTO_LI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_REPOSO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_FLEXION_40 { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_EXTENSION { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_ROTACION_D { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_ROTACION_I { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_LATER_D { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_LD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_LATER_I { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_MOVIMIENTO_LI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_B_COLUMNA_DORSO_LUMBAR_DOLOR_PALPACION { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_EXTENSION_50_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_EXTENSION_50_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_FLEXION_180_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_FLEXION_180_HI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_HOMBRO_DERECHO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_HOMBRO_IZQUIERDO { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_REPOSO_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_REPOSO_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_PALPACION_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_PALPACION_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ABDUCCION_180_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ABDUCCION_180_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ADUCCION_50_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ADUCCION_50_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ROTACION_INTERNA_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ROTACION_INTERNA_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_ROT_INT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_ROT_INT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ROTACION_EXTERNA_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_ROTACION_EXTERNA_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HD_ROT_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_C_HOMBRO_DOLOR_MOVIMIENTO_HI_ROT_EXT { get; set; }


        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_CODO_DERECHO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_CODO_IZQUIERDO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_HD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_HI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_REPOSO_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_REPOSO_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_PALPACION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_PALPACION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_FLEXION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_FLEXION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_EXTENSION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_EXTENSION_CI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CI_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CD_PRO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CI_PRO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_SUPINACION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_SUPINACION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CD_SUP { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_DOLOR_MOVIMIENTO_CI_SUP { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_MUÑECA_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_MUÑECA_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_REPOSO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_REPOSO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_PALPACION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_PALPACION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_FLEXION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_FLEXION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_EXTENSION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_EXTENSION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MI_EXT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_ABDUCCION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_ABDUCCION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MD_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MI_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_ADUCCION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_ADUCCION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MD_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_E_MUÑECA_DOLOR_MOVIMIENTO_MI_AD { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_MANO_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_MANO_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_REPOSO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_REPOSO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_PALPACION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_PALPACION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_FLEXION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_FLEXION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_EXTENSION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_EXTENSION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MI_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_ABDUCCION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_ABDUCCION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MD_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MI_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_ADUCCION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_ADUCCION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MD_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_F_ARTICULACION_METACARPO_FALANGICA_PRIMER_DEDO_DOLOR_MOVIMIENTO_MI_AD { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_MANO_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_MANO_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_REPOSO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_PALPACION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_REPOSO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_PALPACION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_FLEXION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_FLEXION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_MOVIMIENTO_MD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_MOVIMIENTO_MI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_EXTENSION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_EXTENSION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_MOVIMIENTO_MD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_G_ARTICULACION_METACARPO_FALANGICA_1_4_DEDO_DOLOR_MOVIMIENTO_MI_EXT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_MANO_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_MANO_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_REPOSO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_PALPACION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_REPOSO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_PALPACION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_FLEXION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_FLEXION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_MOVIMIENTO_MD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_MOVIMIENTO_MI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_EXTENSION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_EXTENSION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_MOVIMIENTO_MD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_H_ARTICULACION_INTERFALANGICA_PROXIMAL_DOLOR_MOVIMIENTO_MI_EXT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_MANO_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_MANO_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_REPOSO_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_PALPACION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_REPOSO_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_PALPACION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_FLEXION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_FLEXION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_MOVIMIENTO_MD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_MOVIMIENTO_MI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_EXTENSION_MD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_EXTENSION_MI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_MOVIMIENTO_MD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_I_ARTICULACION_INTERFALANGICA_DISTAL_DOLOR_MOVIMIENTO_MI_EXT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_CADERA_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_CADERA_IZQUIERDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_REPOSO_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_REPOSO_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_PALPACION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_PALPACION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_FLEXION_CON_ROD_EXT_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_FLEXION_CON_ROD_EXT_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_FLEX_ROD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_FLEX_ROD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_FLEXION_CON_ROD_FLEX_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_FLEXION_CON_ROD_FLEX_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_FLEX_ROD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_FLEX_ROD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_EXTENSION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_EXTENSION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ABDUCCION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ABDUCCION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_ABD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ADUCCION_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ADUCCION_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_AD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ROTACION_EXTERNA_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ROTACION_EXTERNA_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_ROT_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_ROT_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ROTACION_INTERNA_CD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_ROTACION_INTERNA_CI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CD_ROT_INT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_J_CADERA_DOLOR_MOVIMIENTO_CI_ROT_INT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_RODILLA_DERECHA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_RODILLA_IZQUIRDA { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_REPOSO_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_REPOSO_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_PALPACION_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_PALPACION_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_FLEXION_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_FLEXION_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_MOVIMIENTO_RD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_MOVIMIENTO_RI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_EXTENSION_RD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_EXTENSION_RI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_MOVIMIENTO_RD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_K_RODILLA_DOLOR_MOVIMIENTO_RI_EXT { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_TOBILLO_DERECHO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_TOBILLO_IZQUIERDO { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_REPOSO_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_PALPACION_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_REPOSO_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_PALPACION_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_FLEXION_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TD_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_FLEXION_TI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TI_FLEX { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_EXTENSION_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TD_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_EXTENSION_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TI_EXT { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_INVERSION_SUBTALAR_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TD_IST { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_INVERSION_SUBTALAR_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TI_IST { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_EVERSION_SUBTALAR_TD { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TD_EST { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_EVERSION_SUBTALAR_TI { get; set; }
        public string MÚSCULO_ESQUELÉTICO_2_L_TOBILLO_DOLOR_MOVIMIENTO_TI_EST { get; set; }


        public string Protocolo { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_ServiceComponentId { get; set; }
        public string v_ServiceId { get; set; }


        public string EmpresaPropietaria { get; set; }

        public string EmpresaPropietariaDireccion { get; set; }

        public string EmpresaPropietariaTelefono { get; set; }

        public string EmpresaPropietariaEmail { get; set; }

        public string DetalleAntecEncontrados { get; set; }

        public string Dx { get; set; }

        public string Recomendation { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_PRONACION_CD { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_D_CODO_PRONACION_CI { get; set; }

        public string MÚSCULO_ESQUELÉTICO_2_M_DES_HALLAZGOS { get; set; }

        public string Restriction { get; set; }
    }
}
