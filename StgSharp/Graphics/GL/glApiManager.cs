using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public static partial class glManager
    {
        const int glExtension = 0x1f03;
        const int glVersion = 0x1f02;
        private static readonly string[] glBranch =
            new string[] {
                "OpenGL ES-CM ",
                "OpenGL ES-CL ",
                "OpenGL ES ",
                "OpenGL SC "
            };


        private static string glExtensionList;


        public static unsafe void LoadGLapi(Form form, GLfuncLoader loader)
        {
            if (form.contextID->GetString != IntPtr.Zero)
            {
                return;
            }
            IntPtr proc = IntPtr.Zero;
            proc = loader("glGetString");
            glGetString getString = Marshal.GetDelegateForFunctionPointer<glGetString>(proc);
            form.glContext.GetString = getString;

            string coreVersion;
            if (form.glContext.GetString(glVersion) == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog("Failed to call glGetString", LogType.Error);
            }
            coreVersion = Marshal.PtrToStringAnsi(form.glContext.GetString(glVersion));


            foreach (string branch in glBranch)
            {
                if (coreVersion.Contains(branch))
                {
                    coreVersion.Replace(branch, "");
                }
            }
            string[] versionSep = coreVersion.Split('.');
            int majorVersion = int.Parse(versionSep[0]);
            int minorVersion = int.Parse(versionSep[1]);
            CheckCoreVersion(majorVersion, minorVersion);

            try
            {
                #region load_core_gl

                LoadGLcore10(form.contextID, form.glContext, loader);
                LoadGLcore11(form.contextID, form.glContext, loader);
                LoadGLcore12(form.contextID, form.glContext, loader);
                LoadGLcore13(form.contextID, form.glContext, loader);
                LoadGLcore14(form.contextID, form.glContext, loader);
                LoadGLcore15(form.contextID, form.glContext, loader);
                LoadGLcore20(form.contextID, form.glContext, loader);
                LoadGLcore21(form.contextID, form.glContext, loader);
                LoadGLcore30(form.contextID, form.glContext, loader);
                LoadGLcore31(form.contextID, form.glContext, loader);
                LoadGLcore32(form.contextID, form.glContext, loader);
                LoadGLcore33(form.contextID, form.glContext, loader);
                LoadGLcore40(form.contextID, form.glContext, loader);
                LoadGLcore41(form.contextID, form.glContext, loader);
                LoadGLcore42(form.contextID, form.glContext, loader);
                LoadGLcore43(form.contextID, form.glContext, loader);
                LoadGLcore44(form.contextID, form.glContext, loader);
                LoadGLcore45(form.contextID, form.glContext, loader);
                LoadGLcore46(form.contextID, form.glContext, loader);

                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message != "Final Version") throw;
            }

            IntPtr strPtr = form.glContext.GetString(glExtension);
            string extension;
            if (strPtr == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog("No supported OpenGL extensions are found.", LogType.Info);
                return;
            }
            else
            {
                extension = Marshal.PtrToStringAnsi(strPtr);
            }
            InternalIO.InternalWriteLog("No GL extensions are found.", LogType.Info);

            List<string> extensions =
                extension.Split(" ").ToList();

            #region checkGLextensions
            if (extensions.Contains("gl__4DFX_multisample")) { _4DFX_multisample = true; extensions.Remove("gl_4DFX_multisample"); }
            if (extensions.Contains("gl__3DFX_tbuffer")) { _3DFX_tbuffer = true; extensions.Remove("gl_3DFX_tbuffer"); }
            if (extensions.Contains("gl__3DFX_texture_compression_FXT1")) { _3DFX_texture_compression_FXT1 = true; extensions.Remove("gl_3DFX_texture_compression_FXT1"); }
            if (extensions.Contains("gl_AMD_blend_minmax_factor")) { AMD_blend_minmax_factor = true; extensions.Remove("glAMD_blend_minmax_factor"); }
            if (extensions.Contains("gl_AMD_conservative_depth")) { AMD_conservative_depth = true; extensions.Remove("glAMD_conservative_depth"); }
            if (extensions.Contains("gl_AMD_debug_output")) { AMD_debug_output = true; extensions.Remove("glAMD_debug_output"); }
            if (extensions.Contains("gl_AMD_depth_clamp_separate")) { AMD_depth_clamp_separate = true; extensions.Remove("glAMD_depth_clamp_separate"); }
            if (extensions.Contains("gl_AMD_draw_buffers_blend")) { AMD_draw_buffers_blend = true; extensions.Remove("glAMD_draw_buffers_blend"); }
            if (extensions.Contains("gl_AMD_framebuffer_multisample_advanced")) { AMD_framebuffer_multisample_advanced = true; extensions.Remove("glAMD_framebuffer_multisample_advanced"); }
            if (extensions.Contains("gl_AMD_framebuffer_sample_positions")) { AMD_framebuffer_sample_positions = true; extensions.Remove("glAMD_framebuffer_sample_positions"); }
            if (extensions.Contains("gl_AMD_gcn_shader")) { AMD_gcn_shader = true; extensions.Remove("glAMD_gcn_shader"); }
            if (extensions.Contains("gl_AMD_gpu_shader_half_float")) { AMD_gpu_shader_half_float = true; extensions.Remove("glAMD_gpu_shader_half_float"); }
            if (extensions.Contains("gl_AMD_gpu_shader_int16")) { AMD_gpu_shader_int16 = true; extensions.Remove("glAMD_gpu_shader_int16"); }
            if (extensions.Contains("gl_AMD_gpu_shader_int64")) { AMD_gpu_shader_int64 = true; extensions.Remove("glAMD_gpu_shader_int64"); }
            if (extensions.Contains("gl_AMD_interleaved_elements")) { AMD_interleaved_elements = true; extensions.Remove("glAMD_interleaved_elements"); }
            if (extensions.Contains("gl_AMD_multi_draw_indirect")) { AMD_multi_draw_indirect = true; extensions.Remove("glAMD_multi_draw_indirect"); }
            if (extensions.Contains("gl_AMD_name_gen_delete")) { AMD_name_gen_delete = true; extensions.Remove("glAMD_name_gen_delete"); }
            if (extensions.Contains("gl_AMD_occlusion_query_event")) { AMD_occlusion_query_event = true; extensions.Remove("glAMD_occlusion_query_event"); }
            if (extensions.Contains("gl_AMD_performance_monitor")) { AMD_performance_monitor = true; extensions.Remove("glAMD_performance_monitor"); }
            if (extensions.Contains("gl_AMD_pinned_memory")) { AMD_pinned_memory = true; extensions.Remove("glAMD_pinned_memory"); }
            if (extensions.Contains("gl_AMD_query_buffer_object")) { AMD_query_buffer_object = true; extensions.Remove("glAMD_query_buffer_object"); }
            if (extensions.Contains("gl_AMD_sample_positions")) { AMD_sample_positions = true; extensions.Remove("glAMD_sample_positions"); }
            if (extensions.Contains("gl_AMD_seamless_cubemap_per_texture")) { AMD_seamless_cubemap_per_texture = true; extensions.Remove("glAMD_seamless_cubemap_per_texture"); }
            if (extensions.Contains("gl_AMD_shader_atomic_counter_ops")) { AMD_shader_atomic_counter_ops = true; extensions.Remove("glAMD_shader_atomic_counter_ops"); }
            if (extensions.Contains("gl_AMD_shader_ballot")) { AMD_shader_ballot = true; extensions.Remove("glAMD_shader_ballot"); }
            if (extensions.Contains("gl_AMD_shader_explicit_vertex_parameter")) { AMD_shader_explicit_vertex_parameter = true; extensions.Remove("glAMD_shader_explicit_vertex_parameter"); }
            if (extensions.Contains("gl_AMD_shader_gpu_shader_half_float_fetch")) { AMD_shader_gpu_shader_half_float_fetch = true; extensions.Remove("glAMD_shader_gpu_shader_half_float_fetch"); }
            if (extensions.Contains("gl_AMD_shader_image_load_store_lod")) { AMD_shader_image_load_store_lod = true; extensions.Remove("glAMD_shader_image_load_store_lod"); }
            if (extensions.Contains("gl_AMD_shader_stencil_export")) { AMD_shader_stencil_export = true; extensions.Remove("glAMD_shader_stencil_export"); }
            if (extensions.Contains("gl_AMD_shader_trinary_minmax")) { AMD_shader_trinary_minmax = true; extensions.Remove("glAMD_shader_trinary_minmax"); }
            if (extensions.Contains("gl_AMD_sparse_texture")) { AMD_sparse_texture = true; extensions.Remove("glAMD_sparse_texture"); }
            if (extensions.Contains("gl_AMD_stencil_operation_extended")) { AMD_stencil_operation_extended = true; extensions.Remove("glAMD_stencil_operation_extended"); }
            if (extensions.Contains("gl_AMD_texture_gather_bias_lod")) { AMD_texture_gather_bias_lod = true; extensions.Remove("glAMD_texture_gather_bias_lod"); }
            if (extensions.Contains("gl_AMD_texture_texture4")) { AMD_texture_texture4 = true; extensions.Remove("glAMD_texture_texture4"); }
            if (extensions.Contains("gl_AMD_transform_feedback3_lines_triangles")) { AMD_transform_feedback3_lines_triangles = true; extensions.Remove("glAMD_transform_feedback3_lines_triangles"); }
            if (extensions.Contains("gl_AMD_transform_feedback4")) { AMD_transform_feedback4 = true; extensions.Remove("glAMD_transform_feedback4"); }
            if (extensions.Contains("gl_AMD_vertex_shader_layer")) { AMD_vertex_shader_layer = true; extensions.Remove("glAMD_vertex_shader_layer"); }
            if (extensions.Contains("gl_AMD_vertex_shader_tessellator")) { AMD_vertex_shader_tessellator = true; extensions.Remove("glAMD_vertex_shader_tessellator"); }
            if (extensions.Contains("gl_AMD_vertex_shader_viewport_index")) { AMD_vertex_shader_viewport_index = true; extensions.Remove("glAMD_vertex_shader_viewport_index"); }
            if (extensions.Contains("gl_APPLE_aux_depth_stencil")) { APPLE_aux_depth_stencil = true; extensions.Remove("glAPPLE_aux_depth_stencil"); }
            if (extensions.Contains("gl_APPLE_client_storage")) { APPLE_client_storage = true; extensions.Remove("glAPPLE_client_storage"); }
            if (extensions.Contains("gl_APPLE_element_array")) { APPLE_element_array = true; extensions.Remove("glAPPLE_element_array"); }
            if (extensions.Contains("gl_APPLE_fence")) { APPLE_fence = true; extensions.Remove("glAPPLE_fence"); }
            if (extensions.Contains("gl_APPLE_float_pixels")) { APPLE_float_pixels = true; extensions.Remove("glAPPLE_float_pixels"); }
            if (extensions.Contains("gl_APPLE_flush_buffer_range")) { APPLE_flush_buffer_range = true; extensions.Remove("glAPPLE_flush_buffer_range"); }
            if (extensions.Contains("gl_APPLE_object_purgeable")) { APPLE_object_purgeable = true; extensions.Remove("glAPPLE_object_purgeable"); }
            if (extensions.Contains("gl_APPLE_rgb_422")) { APPLE_rgb_422 = true; extensions.Remove("glAPPLE_rgb_422"); }
            if (extensions.Contains("gl_APPLE_row_bytes")) { APPLE_row_bytes = true; extensions.Remove("glAPPLE_row_bytes"); }
            if (extensions.Contains("gl_APPLE_specular_vector")) { APPLE_specular_vector = true; extensions.Remove("glAPPLE_specular_vector"); }
            if (extensions.Contains("gl_APPLE_texture_range")) { APPLE_texture_range = true; extensions.Remove("glAPPLE_texture_range"); }
            if (extensions.Contains("gl_APPLE_transform_hint")) { APPLE_transform_hint = true; extensions.Remove("glAPPLE_transform_hint"); }
            if (extensions.Contains("gl_APPLE_vertex_array_object")) { APPLE_vertex_array_object = true; extensions.Remove("glAPPLE_vertex_array_object"); }
            if (extensions.Contains("gl_APPLE_vertex_array_range")) { APPLE_vertex_array_range = true; extensions.Remove("glAPPLE_vertex_array_range"); }
            if (extensions.Contains("gl_APPLE_vertex_program_evaluators")) { APPLE_vertex_program_evaluators = true; extensions.Remove("glAPPLE_vertex_program_evaluators"); }
            if (extensions.Contains("gl_APPLE_ycbcr_422")) { APPLE_ycbcr_422 = true; extensions.Remove("glAPPLE_ycbcr_422"); }
            if (extensions.Contains("gl_ARB_ES2_compatibility")) { ARB_ES2_compatibility = true; extensions.Remove("glARB_ES2_compatibility"); }
            if (extensions.Contains("gl_ARB_ES3_1_compatibility")) { ARB_ES3_1_compatibility = true; extensions.Remove("glARB_ES3_1_compatibility"); }
            if (extensions.Contains("gl_ARB_ES3_2_compatibility")) { ARB_ES3_2_compatibility = true; extensions.Remove("glARB_ES3_2_compatibility"); }
            if (extensions.Contains("gl_ARB_ES3_compatibility")) { ARB_ES3_compatibility = true; extensions.Remove("glARB_ES3_compatibility"); }
            if (extensions.Contains("gl_ARB_arrays_of_arrays")) { ARB_arrays_of_arrays = true; extensions.Remove("glARB_arrays_of_arrays"); }
            if (extensions.Contains("gl_ARB_base_instance")) { ARB_base_instance = true; extensions.Remove("glARB_base_instance"); }
            if (extensions.Contains("gl_ARB_bindless_texture")) { ARB_bindless_texture = true; extensions.Remove("glARB_bindless_texture"); }
            if (extensions.Contains("gl_ARB_blend_func_extended")) { ARB_blend_func_extended = true; extensions.Remove("glARB_blend_func_extended"); }
            if (extensions.Contains("gl_ARB_buffer_storage")) { ARB_buffer_storage = true; extensions.Remove("glARB_buffer_storage"); }
            if (extensions.Contains("gl_ARB_cl_event")) { ARB_cl_event = true; extensions.Remove("glARB_cl_event"); }
            if (extensions.Contains("gl_ARB_clear_buffer_object")) { ARB_clear_buffer_object = true; extensions.Remove("glARB_clear_buffer_object"); }
            if (extensions.Contains("gl_ARB_clear_texture")) { ARB_clear_texture = true; extensions.Remove("glARB_clear_texture"); }
            if (extensions.Contains("gl_ARB_clip_control")) { ARB_clip_control = true; extensions.Remove("glARB_clip_control"); }
            if (extensions.Contains("gl_ARB_color_buffer_float")) { ARB_color_buffer_float = true; extensions.Remove("glARB_color_buffer_float"); }
            if (extensions.Contains("gl_ARB_compatibility")) { ARB_compatibility = true; extensions.Remove("glARB_compatibility"); }
            if (extensions.Contains("gl_ARB_compressed_texture_pixel_storage")) { ARB_compressed_texture_pixel_storage = true; extensions.Remove("glARB_compressed_texture_pixel_storage"); }
            if (extensions.Contains("gl_ARB_compute_shader")) { ARB_compute_shader = true; extensions.Remove("glARB_compute_shader"); }
            if (extensions.Contains("gl_ARB_compute_variable_group_size")) { ARB_compute_variable_group_size = true; extensions.Remove("glARB_compute_variable_group_size"); }
            if (extensions.Contains("gl_ARB_conditional_render_inverted")) { ARB_conditional_render_inverted = true; extensions.Remove("glARB_conditional_render_inverted"); }
            if (extensions.Contains("gl_ARB_conservative_depth")) { ARB_conservative_depth = true; extensions.Remove("glARB_conservative_depth"); }
            if (extensions.Contains("gl_ARB_copy_buffer")) { ARB_copy_buffer = true; extensions.Remove("glARB_copy_buffer"); }
            if (extensions.Contains("gl_ARB_copy_image")) { ARB_copy_image = true; extensions.Remove("glARB_copy_image"); }
            if (extensions.Contains("gl_ARB_cull_distance")) { ARB_cull_distance = true; extensions.Remove("glARB_cull_distance"); }
            if (extensions.Contains("gl_ARB_debug_output")) { ARB_debug_output = true; extensions.Remove("glARB_debug_output"); }
            if (extensions.Contains("gl_ARB_depth_buffer_float")) { ARB_depth_buffer_float = true; extensions.Remove("glARB_depth_buffer_float"); }
            if (extensions.Contains("gl_ARB_depth_clamp")) { ARB_depth_clamp = true; extensions.Remove("glARB_depth_clamp"); }
            if (extensions.Contains("gl_ARB_depth_texture")) { ARB_depth_texture = true; extensions.Remove("glARB_depth_texture"); }
            if (extensions.Contains("gl_ARB_derivative_control")) { ARB_derivative_control = true; extensions.Remove("glARB_derivative_control"); }
            if (extensions.Contains("gl_ARB_direct_state_access")) { ARB_direct_state_access = true; extensions.Remove("glARB_direct_state_access"); }
            if (extensions.Contains("gl_ARB_draw_buffers")) { ARB_draw_buffers = true; extensions.Remove("glARB_draw_buffers"); }
            if (extensions.Contains("gl_ARB_draw_buffers_blend")) { ARB_draw_buffers_blend = true; extensions.Remove("glARB_draw_buffers_blend"); }
            if (extensions.Contains("gl_ARB_draw_elements_base_vertex")) { ARB_draw_elements_base_vertex = true; extensions.Remove("glARB_draw_elements_base_vertex"); }
            if (extensions.Contains("gl_ARB_draw_indirect")) { ARB_draw_indirect = true; extensions.Remove("glARB_draw_indirect"); }
            if (extensions.Contains("gl_ARB_draw_instanced")) { ARB_draw_instanced = true; extensions.Remove("glARB_draw_instanced"); }
            if (extensions.Contains("gl_ARB_enhanced_layouts")) { ARB_enhanced_layouts = true; extensions.Remove("glARB_enhanced_layouts"); }
            if (extensions.Contains("gl_ARB_explicit_attrib_location")) { ARB_explicit_attrib_location = true; extensions.Remove("glARB_explicit_attrib_location"); }
            if (extensions.Contains("gl_ARB_explicit_uniform_location")) { ARB_explicit_uniform_location = true; extensions.Remove("glARB_explicit_uniform_location"); }
            if (extensions.Contains("gl_ARB_fragment_coord_conventions")) { ARB_fragment_coord_conventions = true; extensions.Remove("glARB_fragment_coord_conventions"); }
            if (extensions.Contains("gl_ARB_fragment_layer_viewport")) { ARB_fragment_layer_viewport = true; extensions.Remove("glARB_fragment_layer_viewport"); }
            if (extensions.Contains("gl_ARB_fragment_program")) { ARB_fragment_program = true; extensions.Remove("glARB_fragment_program"); }
            if (extensions.Contains("gl_ARB_fragment_program_shadow")) { ARB_fragment_program_shadow = true; extensions.Remove("glARB_fragment_program_shadow"); }
            if (extensions.Contains("gl_ARB_fragment_shader")) { ARB_fragment_shader = true; extensions.Remove("glARB_fragment_shader"); }
            if (extensions.Contains("gl_ARB_fragment_shader_interlock")) { ARB_fragment_shader_interlock = true; extensions.Remove("glARB_fragment_shader_interlock"); }
            if (extensions.Contains("gl_ARB_framebuffer_no_attachments")) { ARB_framebuffer_no_attachments = true; extensions.Remove("glARB_framebuffer_no_attachments"); }
            if (extensions.Contains("gl_ARB_framebuffer_object")) { ARB_framebuffer_object = true; extensions.Remove("glARB_framebuffer_object"); }
            if (extensions.Contains("gl_ARB_framebuffer_sRGB")) { ARB_framebuffer_sRGB = true; extensions.Remove("glARB_framebuffer_sRGB"); }
            if (extensions.Contains("gl_ARB_geometry_shader4")) { ARB_geometry_shader4 = true; extensions.Remove("glARB_geometry_shader4"); }
            if (extensions.Contains("gl_ARB_get_program_binary")) { ARB_get_program_binary = true; extensions.Remove("glARB_get_program_binary"); }
            if (extensions.Contains("gl_ARB_get_texture_sub_image")) { ARB_get_texture_sub_image = true; extensions.Remove("glARB_get_texture_sub_image"); }
            if (extensions.Contains("gl_ARB_gl_spirv")) { ARB_gl_spirv = true; extensions.Remove("glARB_gl_spirv"); }
            if (extensions.Contains("gl_ARB_gpu_shader5")) { ARB_gpu_shader5 = true; extensions.Remove("glARB_gpu_shader5"); }
            if (extensions.Contains("gl_ARB_gpu_shader_fp64")) { ARB_gpu_shader_fp64 = true; extensions.Remove("glARB_gpu_shader_fp64"); }
            if (extensions.Contains("gl_ARB_gpu_shader_int64")) { ARB_gpu_shader_int64 = true; extensions.Remove("glARB_gpu_shader_int64"); }
            if (extensions.Contains("gl_ARB_half_float_pixel")) { ARB_half_float_pixel = true; extensions.Remove("glARB_half_float_pixel"); }
            if (extensions.Contains("gl_ARB_half_float_vertex")) { ARB_half_float_vertex = true; extensions.Remove("glARB_half_float_vertex"); }
            if (extensions.Contains("gl_ARB_imaging")) { ARB_imaging = true; extensions.Remove("glARB_imaging"); }
            if (extensions.Contains("gl_ARB_indirect_parameters")) { ARB_indirect_parameters = true; extensions.Remove("glARB_indirect_parameters"); }
            if (extensions.Contains("gl_ARB_instanced_arrays")) { ARB_instanced_arrays = true; extensions.Remove("glARB_instanced_arrays"); }
            if (extensions.Contains("gl_ARB_internalformat_query")) { ARB_internalformat_query = true; extensions.Remove("glARB_internalformat_query"); }
            if (extensions.Contains("gl_ARB_internalformat_query2")) { ARB_internalformat_query2 = true; extensions.Remove("glARB_internalformat_query2"); }
            if (extensions.Contains("gl_ARB_invalidate_subdata")) { ARB_invalidate_subdata = true; extensions.Remove("glARB_invalidate_subdata"); }
            if (extensions.Contains("gl_ARB_map_buffer_alignment")) { ARB_map_buffer_alignment = true; extensions.Remove("glARB_map_buffer_alignment"); }
            if (extensions.Contains("gl_ARB_map_buffer_range")) { ARB_map_buffer_range = true; extensions.Remove("glARB_map_buffer_range"); }
            if (extensions.Contains("gl_ARB_matrix_palette")) { ARB_matrix_palette = true; extensions.Remove("glARB_matrix_palette"); }
            if (extensions.Contains("gl_ARB_multi_bind")) { ARB_multi_bind = true; extensions.Remove("glARB_multi_bind"); }
            if (extensions.Contains("gl_ARB_multi_draw_indirect")) { ARB_multi_draw_indirect = true; extensions.Remove("glARB_multi_draw_indirect"); }
            if (extensions.Contains("gl_ARB_multisample")) { ARB_multisample = true; extensions.Remove("glARB_multisample"); }
            if (extensions.Contains("gl_ARB_multitexture")) { ARB_multitexture = true; extensions.Remove("glARB_multitexture"); }
            if (extensions.Contains("gl_ARB_occlusion_query")) { ARB_occlusion_query = true; extensions.Remove("glARB_occlusion_query"); }
            if (extensions.Contains("gl_ARB_occlusion_query2")) { ARB_occlusion_query2 = true; extensions.Remove("glARB_occlusion_query2"); }
            if (extensions.Contains("gl_ARB_parallel_shader_compile")) { ARB_parallel_shader_compile = true; extensions.Remove("glARB_parallel_shader_compile"); }
            if (extensions.Contains("gl_ARB_pipeline_statistics_query")) { ARB_pipeline_statistics_query = true; extensions.Remove("glARB_pipeline_statistics_query"); }
            if (extensions.Contains("gl_ARB_pixel_buffer_object")) { ARB_pixel_buffer_object = true; extensions.Remove("glARB_pixel_buffer_object"); }
            if (extensions.Contains("gl_ARB_point_parameters")) { ARB_point_parameters = true; extensions.Remove("glARB_point_parameters"); }
            if (extensions.Contains("gl_ARB_point_sprite")) { ARB_point_sprite = true; extensions.Remove("glARB_point_sprite"); }
            if (extensions.Contains("gl_ARB_polygon_offset_clamp")) { ARB_polygon_offset_clamp = true; extensions.Remove("glARB_polygon_offset_clamp"); }
            if (extensions.Contains("gl_ARB_post_depth_coverage")) { ARB_post_depth_coverage = true; extensions.Remove("glARB_post_depth_coverage"); }
            if (extensions.Contains("gl_ARB_program_interface_query")) { ARB_program_interface_query = true; extensions.Remove("glARB_program_interface_query"); }
            if (extensions.Contains("gl_ARB_provoking_vertex")) { ARB_provoking_vertex = true; extensions.Remove("glARB_provoking_vertex"); }
            if (extensions.Contains("gl_ARB_query_buffer_object")) { ARB_query_buffer_object = true; extensions.Remove("glARB_query_buffer_object"); }
            if (extensions.Contains("gl_ARB_robust_buffer_access_behavior")) { ARB_robust_buffer_access_behavior = true; extensions.Remove("glARB_robust_buffer_access_behavior"); }
            if (extensions.Contains("gl_ARB_robustness")) { ARB_robustness = true; extensions.Remove("glARB_robustness"); }
            if (extensions.Contains("gl_ARB_robustness_isolation")) { ARB_robustness_isolation = true; extensions.Remove("glARB_robustness_isolation"); }
            if (extensions.Contains("gl_ARB_sample_locations")) { ARB_sample_locations = true; extensions.Remove("glARB_sample_locations"); }
            if (extensions.Contains("gl_ARB_sample_shading")) { ARB_sample_shading = true; extensions.Remove("glARB_sample_shading"); }
            if (extensions.Contains("gl_ARB_sampler_objects")) { ARB_sampler_objects = true; extensions.Remove("glARB_sampler_objects"); }
            if (extensions.Contains("gl_ARB_seamless_cube_map")) { ARB_seamless_cube_map = true; extensions.Remove("glARB_seamless_cube_map"); }
            if (extensions.Contains("gl_ARB_seamless_cubemap_per_texture")) { ARB_seamless_cubemap_per_texture = true; extensions.Remove("glARB_seamless_cubemap_per_texture"); }
            if (extensions.Contains("gl_ARB_separate_shader_objects")) { ARB_separate_shader_objects = true; extensions.Remove("glARB_separate_shader_objects"); }
            if (extensions.Contains("gl_ARB_shader_atomic_counter_ops")) { ARB_shader_atomic_counter_ops = true; extensions.Remove("glARB_shader_atomic_counter_ops"); }
            if (extensions.Contains("gl_ARB_shader_atomic_counters")) { ARB_shader_atomic_counters = true; extensions.Remove("glARB_shader_atomic_counters"); }
            if (extensions.Contains("gl_ARB_shader_ballot")) { ARB_shader_ballot = true; extensions.Remove("glARB_shader_ballot"); }
            if (extensions.Contains("gl_ARB_shader_bit_encoding")) { ARB_shader_bit_encoding = true; extensions.Remove("glARB_shader_bit_encoding"); }
            if (extensions.Contains("gl_ARB_shader_clock")) { ARB_shader_clock = true; extensions.Remove("glARB_shader_clock"); }
            if (extensions.Contains("gl_ARB_shader_draw_parameters")) { ARB_shader_draw_parameters = true; extensions.Remove("glARB_shader_draw_parameters"); }
            if (extensions.Contains("gl_ARB_shader_group_vote")) { ARB_shader_group_vote = true; extensions.Remove("glARB_shader_group_vote"); }
            if (extensions.Contains("gl_ARB_shader_image_load_store")) { ARB_shader_image_load_store = true; extensions.Remove("glARB_shader_image_load_store"); }
            if (extensions.Contains("gl_ARB_shader_image_size")) { ARB_shader_image_size = true; extensions.Remove("glARB_shader_image_size"); }
            if (extensions.Contains("gl_ARB_shader_objects")) { ARB_shader_objects = true; extensions.Remove("glARB_shader_objects"); }
            if (extensions.Contains("gl_ARB_shader_precision")) { ARB_shader_precision = true; extensions.Remove("glARB_shader_precision"); }
            if (extensions.Contains("gl_ARB_shader_stencil_export")) { ARB_shader_stencil_export = true; extensions.Remove("glARB_shader_stencil_export"); }
            if (extensions.Contains("gl_ARB_shader_storage_buffer_object")) { ARB_shader_storage_buffer_object = true; extensions.Remove("glARB_shader_storage_buffer_object"); }
            if (extensions.Contains("gl_ARB_shader_subroutine")) { ARB_shader_subroutine = true; extensions.Remove("glARB_shader_subroutine"); }
            if (extensions.Contains("gl_ARB_shader_texture_image_samples")) { ARB_shader_texture_image_samples = true; extensions.Remove("glARB_shader_texture_image_samples"); }
            if (extensions.Contains("gl_ARB_shader_texture_lod")) { ARB_shader_texture_lod = true; extensions.Remove("glARB_shader_texture_lod"); }
            if (extensions.Contains("gl_ARB_shader_viewport_layer_array")) { ARB_shader_viewport_layer_array = true; extensions.Remove("glARB_shader_viewport_layer_array"); }
            if (extensions.Contains("gl_ARB_shading_language_100")) { ARB_shading_language_100 = true; extensions.Remove("glARB_shading_language_100"); }
            if (extensions.Contains("gl_ARB_shading_language_420pack")) { ARB_shading_language_420pack = true; extensions.Remove("glARB_shading_language_420pack"); }
            if (extensions.Contains("gl_ARB_shading_language_include")) { ARB_shading_language_include = true; extensions.Remove("glARB_shading_language_include"); }
            if (extensions.Contains("gl_ARB_shading_language_packing")) { ARB_shading_language_packing = true; extensions.Remove("glARB_shading_language_packing"); }
            if (extensions.Contains("gl_ARB_shadow")) { ARB_shadow = true; extensions.Remove("glARB_shadow"); }
            if (extensions.Contains("gl_ARB_shadow_ambient")) { ARB_shadow_ambient = true; extensions.Remove("glARB_shadow_ambient"); }
            if (extensions.Contains("gl_ARB_sparse_buffer")) { ARB_sparse_buffer = true; extensions.Remove("glARB_sparse_buffer"); }
            if (extensions.Contains("gl_ARB_sparse_texture")) { ARB_sparse_texture = true; extensions.Remove("glARB_sparse_texture"); }
            if (extensions.Contains("gl_ARB_sparse_texture2")) { ARB_sparse_texture2 = true; extensions.Remove("glARB_sparse_texture2"); }
            if (extensions.Contains("gl_ARB_sparse_texture_clamp")) { ARB_sparse_texture_clamp = true; extensions.Remove("glARB_sparse_texture_clamp"); }
            if (extensions.Contains("gl_ARB_spirv_extensions")) { ARB_spirv_extensions = true; extensions.Remove("glARB_spirv_extensions"); }
            if (extensions.Contains("gl_ARB_stencil_texturing")) { ARB_stencil_texturing = true; extensions.Remove("glARB_stencil_texturing"); }
            if (extensions.Contains("gl_ARB_sync")) { ARB_sync = true; extensions.Remove("glARB_sync"); }
            if (extensions.Contains("gl_ARB_tessellation_shader")) { ARB_tessellation_shader = true; extensions.Remove("glARB_tessellation_shader"); }
            if (extensions.Contains("gl_ARB_texture_barrier")) { ARB_texture_barrier = true; extensions.Remove("glARB_texture_barrier"); }
            if (extensions.Contains("gl_ARB_texture_border_clamp")) { ARB_texture_border_clamp = true; extensions.Remove("glARB_texture_border_clamp"); }
            if (extensions.Contains("gl_ARB_texture_buffer_object")) { ARB_texture_buffer_object = true; extensions.Remove("glARB_texture_buffer_object"); }
            if (extensions.Contains("gl_ARB_texture_buffer_object_rgb32")) { ARB_texture_buffer_object_rgb32 = true; extensions.Remove("glARB_texture_buffer_object_rgb32"); }
            if (extensions.Contains("gl_ARB_texture_buffer_range")) { ARB_texture_buffer_range = true; extensions.Remove("glARB_texture_buffer_range"); }
            if (extensions.Contains("gl_ARB_texture_compression")) { ARB_texture_compression = true; extensions.Remove("glARB_texture_compression"); }
            if (extensions.Contains("gl_ARB_texture_compression_bptc")) { ARB_texture_compression_bptc = true; extensions.Remove("glARB_texture_compression_bptc"); }
            if (extensions.Contains("gl_ARB_texture_compression_rgtc")) { ARB_texture_compression_rgtc = true; extensions.Remove("glARB_texture_compression_rgtc"); }
            if (extensions.Contains("gl_ARB_texture_cube_map")) { ARB_texture_cube_map = true; extensions.Remove("glARB_texture_cube_map"); }
            if (extensions.Contains("gl_ARB_texture_cube_map_array")) { ARB_texture_cube_map_array = true; extensions.Remove("glARB_texture_cube_map_array"); }
            if (extensions.Contains("gl_ARB_texture_env_add")) { ARB_texture_env_add = true; extensions.Remove("glARB_texture_env_add"); }
            if (extensions.Contains("gl_ARB_texture_env_combine")) { ARB_texture_env_combine = true; extensions.Remove("glARB_texture_env_combine"); }
            if (extensions.Contains("gl_ARB_texture_env_crossbar")) { ARB_texture_env_crossbar = true; extensions.Remove("glARB_texture_env_crossbar"); }
            if (extensions.Contains("gl_ARB_texture_env_dot3")) { ARB_texture_env_dot3 = true; extensions.Remove("glARB_texture_env_dot3"); }
            if (extensions.Contains("gl_ARB_texture_filter_anisotropic")) { ARB_texture_filter_anisotropic = true; extensions.Remove("glARB_texture_filter_anisotropic"); }
            if (extensions.Contains("gl_ARB_texture_filter_minmax")) { ARB_texture_filter_minmax = true; extensions.Remove("glARB_texture_filter_minmax"); }
            if (extensions.Contains("gl_ARB_texture_float")) { ARB_texture_float = true; extensions.Remove("glARB_texture_float"); }
            if (extensions.Contains("gl_ARB_texture_gather")) { ARB_texture_gather = true; extensions.Remove("glARB_texture_gather"); }
            if (extensions.Contains("gl_ARB_texture_mirror_clamp_to_edge")) { ARB_texture_mirror_clamp_to_edge = true; extensions.Remove("glARB_texture_mirror_clamp_to_edge"); }
            if (extensions.Contains("gl_ARB_texture_mirrored_repeat")) { ARB_texture_mirrored_repeat = true; extensions.Remove("glARB_texture_mirrored_repeat"); }
            if (extensions.Contains("gl_ARB_texture_multisample")) { ARB_texture_multisample = true; extensions.Remove("glARB_texture_multisample"); }
            if (extensions.Contains("gl_ARB_texture_non_power_of_two")) { ARB_texture_non_power_of_two = true; extensions.Remove("glARB_texture_non_power_of_two"); }
            if (extensions.Contains("gl_ARB_texture_query_levels")) { ARB_texture_query_levels = true; extensions.Remove("glARB_texture_query_levels"); }
            if (extensions.Contains("gl_ARB_texture_query_lod")) { ARB_texture_query_lod = true; extensions.Remove("glARB_texture_query_lod"); }
            if (extensions.Contains("gl_ARB_texture_rectangle")) { ARB_texture_rectangle = true; extensions.Remove("glARB_texture_rectangle"); }
            if (extensions.Contains("gl_ARB_texture_rg")) { ARB_texture_rg = true; extensions.Remove("glARB_texture_rg"); }
            if (extensions.Contains("gl_ARB_texture_rgb10_a2ui")) { ARB_texture_rgb10_a2ui = true; extensions.Remove("glARB_texture_rgb10_a2ui"); }
            if (extensions.Contains("gl_ARB_texture_stencil8")) { ARB_texture_stencil8 = true; extensions.Remove("glARB_texture_stencil8"); }
            if (extensions.Contains("gl_ARB_texture_storage")) { ARB_texture_storage = true; extensions.Remove("glARB_texture_storage"); }
            if (extensions.Contains("gl_ARB_texture_storage_multisample")) { ARB_texture_storage_multisample = true; extensions.Remove("glARB_texture_storage_multisample"); }
            if (extensions.Contains("gl_ARB_texture_swizzle")) { ARB_texture_swizzle = true; extensions.Remove("glARB_texture_swizzle"); }
            if (extensions.Contains("gl_ARB_texture_view")) { ARB_texture_view = true; extensions.Remove("glARB_texture_view"); }
            if (extensions.Contains("gl_ARB_timer_query")) { ARB_timer_query = true; extensions.Remove("glARB_timer_query"); }
            if (extensions.Contains("gl_ARB_transform_feedback2")) { ARB_transform_feedback2 = true; extensions.Remove("glARB_transform_feedback2"); }
            if (extensions.Contains("gl_ARB_transform_feedback3")) { ARB_transform_feedback3 = true; extensions.Remove("glARB_transform_feedback3"); }
            if (extensions.Contains("gl_ARB_transform_feedback_instanced")) { ARB_transform_feedback_instanced = true; extensions.Remove("glARB_transform_feedback_instanced"); }
            if (extensions.Contains("gl_ARB_transform_feedback_overflow_query")) { ARB_transform_feedback_overflow_query = true; extensions.Remove("glARB_transform_feedback_overflow_query"); }
            if (extensions.Contains("gl_ARB_transpose_matrix")) { ARB_transpose_matrix = true; extensions.Remove("glARB_transpose_matrix"); }
            if (extensions.Contains("gl_ARB_uniform_buffer_object")) { ARB_uniform_buffer_object = true; extensions.Remove("glARB_uniform_buffer_object"); }
            if (extensions.Contains("gl_ARB_vertex_array_bgra")) { ARB_vertex_array_bgra = true; extensions.Remove("glARB_vertex_array_bgra"); }
            if (extensions.Contains("gl_ARB_vertex_array_object")) { ARB_vertex_array_object = true; extensions.Remove("glARB_vertex_array_object"); }
            if (extensions.Contains("gl_ARB_vertex_attrib_64bit")) { ARB_vertex_attrib_64bit = true; extensions.Remove("glARB_vertex_attrib_64bit"); }
            if (extensions.Contains("gl_ARB_vertex_attrib_binding")) { ARB_vertex_attrib_binding = true; extensions.Remove("glARB_vertex_attrib_binding"); }
            if (extensions.Contains("gl_ARB_vertex_blend")) { ARB_vertex_blend = true; extensions.Remove("glARB_vertex_blend"); }
            if (extensions.Contains("gl_ARB_vertex_buffer_object")) { ARB_vertex_buffer_object = true; extensions.Remove("glARB_vertex_buffer_object"); }
            if (extensions.Contains("gl_ARB_vertex_program")) { ARB_vertex_program = true; extensions.Remove("glARB_vertex_program"); }
            if (extensions.Contains("gl_ARB_vertex_shader")) { ARB_vertex_shader = true; extensions.Remove("glARB_vertex_shader"); }
            if (extensions.Contains("gl_ARB_vertex_type_10f_11f_11f_rev")) { ARB_vertex_type_10f_11f_11f_rev = true; extensions.Remove("glARB_vertex_type_10f_11f_11f_rev"); }
            if (extensions.Contains("gl_ARB_vertex_type_2_10_10_10_rev")) { ARB_vertex_type_2_10_10_10_rev = true; extensions.Remove("glARB_vertex_type_2_10_10_10_rev"); }
            if (extensions.Contains("gl_ARB_viewport_array")) { ARB_viewport_array = true; extensions.Remove("glARB_viewport_array"); }
            if (extensions.Contains("gl_ARB_window_pos")) { ARB_window_pos = true; extensions.Remove("glARB_window_pos"); }
            if (extensions.Contains("gl_ATI_draw_buffers")) { ATI_draw_buffers = true; extensions.Remove("glATI_draw_buffers"); }
            if (extensions.Contains("gl_ATI_element_array")) { ATI_element_array = true; extensions.Remove("glATI_element_array"); }
            if (extensions.Contains("gl_ATI_envmap_bumpmap")) { ATI_envmap_bumpmap = true; extensions.Remove("glATI_envmap_bumpmap"); }
            if (extensions.Contains("gl_ATI_fragment_shader")) { ATI_fragment_shader = true; extensions.Remove("glATI_fragment_shader"); }
            if (extensions.Contains("gl_ATI_map_object_buffer")) { ATI_map_object_buffer = true; extensions.Remove("glATI_map_object_buffer"); }
            if (extensions.Contains("gl_ATI_meminfo")) { ATI_meminfo = true; extensions.Remove("glATI_meminfo"); }
            if (extensions.Contains("gl_ATI_pixel_format_float")) { ATI_pixel_format_float = true; extensions.Remove("glATI_pixel_format_float"); }
            if (extensions.Contains("gl_ATI_pn_triangles")) { ATI_pn_triangles = true; extensions.Remove("glATI_pn_triangles"); }
            if (extensions.Contains("gl_ATI_separate_stencil")) { ATI_separate_stencil = true; extensions.Remove("glATI_separate_stencil"); }
            if (extensions.Contains("gl_ATI_text_fragment_shader")) { ATI_text_fragment_shader = true; extensions.Remove("glATI_text_fragment_shader"); }
            if (extensions.Contains("gl_ATI_texture_env_combine3")) { ATI_texture_env_combine3 = true; extensions.Remove("glATI_texture_env_combine3"); }
            if (extensions.Contains("gl_ATI_texture_float")) { ATI_texture_float = true; extensions.Remove("glATI_texture_float"); }
            if (extensions.Contains("gl_ATI_texture_mirror_once")) { ATI_texture_mirror_once = true; extensions.Remove("glATI_texture_mirror_once"); }
            if (extensions.Contains("gl_ATI_vertex_array_object")) { ATI_vertex_array_object = true; extensions.Remove("glATI_vertex_array_object"); }
            if (extensions.Contains("gl_ATI_vertex_attrib_array_object")) { ATI_vertex_attrib_array_object = true; extensions.Remove("glATI_vertex_attrib_array_object"); }
            if (extensions.Contains("gl_ATI_vertex_streams")) { ATI_vertex_streams = true; extensions.Remove("glATI_vertex_streams"); }
            if (extensions.Contains("gl_EXT_422_pixels")) { EXT_422_pixels = true; extensions.Remove("glEXT_422_pixels"); }
            if (extensions.Contains("gl_EXT_EGL_image_storage")) { EXT_EGL_image_storage = true; extensions.Remove("glEXT_EGL_image_storage"); }
            if (extensions.Contains("gl_EXT_EGL_sync")) { EXT_EGL_sync = true; extensions.Remove("glEXT_EGL_sync"); }
            if (extensions.Contains("gl_EXT_abgr")) { EXT_abgr = true; extensions.Remove("glEXT_abgr"); }
            if (extensions.Contains("gl_EXT_bgra")) { EXT_bgra = true; extensions.Remove("glEXT_bgra"); }
            if (extensions.Contains("gl_EXT_bindable_uniform")) { EXT_bindable_uniform = true; extensions.Remove("glEXT_bindable_uniform"); }
            if (extensions.Contains("gl_EXT_blend_color")) { EXT_blend_color = true; extensions.Remove("glEXT_blend_color"); }
            if (extensions.Contains("gl_EXT_blend_equation_separate")) { EXT_blend_equation_separate = true; extensions.Remove("glEXT_blend_equation_separate"); }
            if (extensions.Contains("gl_EXT_blend_func_separate")) { EXT_blend_func_separate = true; extensions.Remove("glEXT_blend_func_separate"); }
            if (extensions.Contains("gl_EXT_blend_logic_op")) { EXT_blend_logic_op = true; extensions.Remove("glEXT_blend_logic_op"); }
            if (extensions.Contains("gl_EXT_blend_minmax")) { EXT_blend_minmax = true; extensions.Remove("glEXT_blend_minmax"); }
            if (extensions.Contains("gl_EXT_blend_subtract")) { EXT_blend_subtract = true; extensions.Remove("glEXT_blend_subtract"); }
            if (extensions.Contains("gl_EXT_clip_volume_hint")) { EXT_clip_volume_hint = true; extensions.Remove("glEXT_clip_volume_hint"); }
            if (extensions.Contains("gl_EXT_cmyka")) { EXT_cmyka = true; extensions.Remove("glEXT_cmyka"); }
            if (extensions.Contains("gl_EXT_color_subtable")) { EXT_color_subtable = true; extensions.Remove("glEXT_color_subtable"); }
            if (extensions.Contains("gl_EXT_compiled_vertex_array")) { EXT_compiled_vertex_array = true; extensions.Remove("glEXT_compiled_vertex_array"); }
            if (extensions.Contains("gl_EXT_convolution")) { EXT_convolution = true; extensions.Remove("glEXT_convolution"); }
            if (extensions.Contains("gl_EXT_coordinate_frame")) { EXT_coordinate_frame = true; extensions.Remove("glEXT_coordinate_frame"); }
            if (extensions.Contains("gl_EXT_copy_texture")) { EXT_copy_texture = true; extensions.Remove("glEXT_copy_texture"); }
            if (extensions.Contains("gl_EXT_cull_vertex")) { EXT_cull_vertex = true; extensions.Remove("glEXT_cull_vertex"); }
            if (extensions.Contains("gl_EXT_debug_label")) { EXT_debug_label = true; extensions.Remove("glEXT_debug_label"); }
            if (extensions.Contains("gl_EXT_debug_marker")) { EXT_debug_marker = true; extensions.Remove("glEXT_debug_marker"); }
            if (extensions.Contains("gl_EXT_depth_bounds_test")) { EXT_depth_bounds_test = true; extensions.Remove("glEXT_depth_bounds_test"); }
            if (extensions.Contains("gl_EXT_direct_state_access")) { EXT_direct_state_access = true; extensions.Remove("glEXT_direct_state_access"); }
            if (extensions.Contains("gl_EXT_draw_buffers2")) { EXT_draw_buffers2 = true; extensions.Remove("glEXT_draw_buffers2"); }
            if (extensions.Contains("gl_EXT_draw_instanced")) { EXT_draw_instanced = true; extensions.Remove("glEXT_draw_instanced"); }
            if (extensions.Contains("gl_EXT_draw_range_elements")) { EXT_draw_range_elements = true; extensions.Remove("glEXT_draw_range_elements"); }
            if (extensions.Contains("gl_EXT_external_buffer")) { EXT_external_buffer = true; extensions.Remove("glEXT_external_buffer"); }
            if (extensions.Contains("gl_EXT_fog_coord")) { EXT_fog_coord = true; extensions.Remove("glEXT_fog_coord"); }
            if (extensions.Contains("gl_EXT_framebuffer_blit")) { EXT_framebuffer_blit = true; extensions.Remove("glEXT_framebuffer_blit"); }
            if (extensions.Contains("gl_EXT_framebuffer_blit_layers")) { EXT_framebuffer_blit_layers = true; extensions.Remove("glEXT_framebuffer_blit_layers"); }
            if (extensions.Contains("gl_EXT_framebuffer_multisample")) { EXT_framebuffer_multisample = true; extensions.Remove("glEXT_framebuffer_multisample"); }
            if (extensions.Contains("gl_EXT_framebuffer_multisample_blit_scaled")) { EXT_framebuffer_multisample_blit_scaled = true; extensions.Remove("glEXT_framebuffer_multisample_blit_scaled"); }
            if (extensions.Contains("gl_EXT_framebuffer_object")) { EXT_framebuffer_object = true; extensions.Remove("glEXT_framebuffer_object"); }
            if (extensions.Contains("gl_EXT_framebuffer_sRGB")) { EXT_framebuffer_sRGB = true; extensions.Remove("glEXT_framebuffer_sRGB"); }
            if (extensions.Contains("gl_EXT_geometry_shader4")) { EXT_geometry_shader4 = true; extensions.Remove("glEXT_geometry_shader4"); }
            if (extensions.Contains("gl_EXT_gpu_program_parameters")) { EXT_gpu_program_parameters = true; extensions.Remove("glEXT_gpu_program_parameters"); }
            if (extensions.Contains("gl_EXT_gpu_shader4")) { EXT_gpu_shader4 = true; extensions.Remove("glEXT_gpu_shader4"); }
            if (extensions.Contains("gl_EXT_histogram")) { EXT_histogram = true; extensions.Remove("glEXT_histogram"); }
            if (extensions.Contains("gl_EXT_index_array_formats")) { EXT_index_array_formats = true; extensions.Remove("glEXT_index_array_formats"); }
            if (extensions.Contains("gl_EXT_index_func")) { EXT_index_func = true; extensions.Remove("glEXT_index_func"); }
            if (extensions.Contains("gl_EXT_index_material")) { EXT_index_material = true; extensions.Remove("glEXT_index_material"); }
            if (extensions.Contains("gl_EXT_index_texture")) { EXT_index_texture = true; extensions.Remove("glEXT_index_texture"); }
            if (extensions.Contains("gl_EXT_light_texture")) { EXT_light_texture = true; extensions.Remove("glEXT_light_texture"); }
            if (extensions.Contains("gl_EXT_memory_object")) { EXT_memory_object = true; extensions.Remove("glEXT_memory_object"); }
            if (extensions.Contains("gl_EXT_memory_object_fd")) { EXT_memory_object_fd = true; extensions.Remove("glEXT_memory_object_fd"); }
            if (extensions.Contains("gl_EXT_memory_object_win32")) { EXT_memory_object_win32 = true; extensions.Remove("glEXT_memory_object_win32"); }
            if (extensions.Contains("gl_EXT_misc_attribute")) { EXT_misc_attribute = true; extensions.Remove("glEXT_misc_attribute"); }
            if (extensions.Contains("gl_EXT_multi_draw_arrays")) { EXT_multi_draw_arrays = true; extensions.Remove("glEXT_multi_draw_arrays"); }
            if (extensions.Contains("gl_EXT_multisample")) { EXT_multisample = true; extensions.Remove("glEXT_multisample"); }
            if (extensions.Contains("gl_EXT_multiview_tessellation_geometry_shader")) { EXT_multiview_tessellation_geometry_shader = true; extensions.Remove("glEXT_multiview_tessellation_geometry_shader"); }
            if (extensions.Contains("gl_EXT_multiview_texture_multisample")) { EXT_multiview_texture_multisample = true; extensions.Remove("glEXT_multiview_texture_multisample"); }
            if (extensions.Contains("gl_EXT_multiview_timer_query")) { EXT_multiview_timer_query = true; extensions.Remove("glEXT_multiview_timer_query"); }
            if (extensions.Contains("gl_EXT_packed_depth_stencil")) { EXT_packed_depth_stencil = true; extensions.Remove("glEXT_packed_depth_stencil"); }
            if (extensions.Contains("gl_EXT_packed_float")) { EXT_packed_float = true; extensions.Remove("glEXT_packed_float"); }
            if (extensions.Contains("gl_EXT_packed_pixels")) { EXT_packed_pixels = true; extensions.Remove("glEXT_packed_pixels"); }
            if (extensions.Contains("gl_EXT_paletted_texture")) { EXT_paletted_texture = true; extensions.Remove("glEXT_paletted_texture"); }
            if (extensions.Contains("gl_EXT_pixel_buffer_object")) { EXT_pixel_buffer_object = true; extensions.Remove("glEXT_pixel_buffer_object"); }
            if (extensions.Contains("gl_EXT_pixel_transform")) { EXT_pixel_transform = true; extensions.Remove("glEXT_pixel_transform"); }
            if (extensions.Contains("gl_EXT_pixel_transform_color_table")) { EXT_pixel_transform_color_table = true; extensions.Remove("glEXT_pixel_transform_color_table"); }
            if (extensions.Contains("gl_EXT_point_parameters")) { EXT_point_parameters = true; extensions.Remove("glEXT_point_parameters"); }
            if (extensions.Contains("gl_EXT_polygon_offset")) { EXT_polygon_offset = true; extensions.Remove("glEXT_polygon_offset"); }
            if (extensions.Contains("gl_EXT_polygon_offset_clamp")) { EXT_polygon_offset_clamp = true; extensions.Remove("glEXT_polygon_offset_clamp"); }
            if (extensions.Contains("gl_EXT_post_depth_coverage")) { EXT_post_depth_coverage = true; extensions.Remove("glEXT_post_depth_coverage"); }
            if (extensions.Contains("gl_EXT_provoking_vertex")) { EXT_provoking_vertex = true; extensions.Remove("glEXT_provoking_vertex"); }
            if (extensions.Contains("gl_EXT_raster_multisample")) { EXT_raster_multisample = true; extensions.Remove("glEXT_raster_multisample"); }
            if (extensions.Contains("gl_EXT_rescale_normal")) { EXT_rescale_normal = true; extensions.Remove("glEXT_rescale_normal"); }
            if (extensions.Contains("gl_EXT_secondary_color")) { EXT_secondary_color = true; extensions.Remove("glEXT_secondary_color"); }
            if (extensions.Contains("gl_EXT_semaphore")) { EXT_semaphore = true; extensions.Remove("glEXT_semaphore"); }
            if (extensions.Contains("gl_EXT_semaphore_fd")) { EXT_semaphore_fd = true; extensions.Remove("glEXT_semaphore_fd"); }
            if (extensions.Contains("gl_EXT_semaphore_win32")) { EXT_semaphore_win32 = true; extensions.Remove("glEXT_semaphore_win32"); }
            if (extensions.Contains("gl_EXT_separate_shader_objects")) { EXT_separate_shader_objects = true; extensions.Remove("glEXT_separate_shader_objects"); }
            if (extensions.Contains("gl_EXT_separate_specular_color")) { EXT_separate_specular_color = true; extensions.Remove("glEXT_separate_specular_color"); }
            if (extensions.Contains("gl_EXT_shader_framebuffer_fetch")) { EXT_shader_framebuffer_fetch = true; extensions.Remove("glEXT_shader_framebuffer_fetch"); }
            if (extensions.Contains("gl_EXT_shader_framebuffer_fetch_non_coherent")) { EXT_shader_framebuffer_fetch_non_coherent = true; extensions.Remove("glEXT_shader_framebuffer_fetch_non_coherent"); }
            if (extensions.Contains("gl_EXT_shader_image_load_formatted")) { EXT_shader_image_load_formatted = true; extensions.Remove("glEXT_shader_image_load_formatted"); }
            if (extensions.Contains("gl_EXT_shader_image_load_store")) { EXT_shader_image_load_store = true; extensions.Remove("glEXT_shader_image_load_store"); }
            if (extensions.Contains("gl_EXT_shader_integer_mix")) { EXT_shader_integer_mix = true; extensions.Remove("glEXT_shader_integer_mix"); }
            if (extensions.Contains("gl_EXT_shader_samples_identical")) { EXT_shader_samples_identical = true; extensions.Remove("glEXT_shader_samples_identical"); }
            if (extensions.Contains("gl_EXT_shadow_funcs")) { EXT_shadow_funcs = true; extensions.Remove("glEXT_shadow_funcs"); }
            if (extensions.Contains("gl_EXT_shared_texture_palette")) { EXT_shared_texture_palette = true; extensions.Remove("glEXT_shared_texture_palette"); }
            if (extensions.Contains("gl_EXT_sparse_texture2")) { EXT_sparse_texture2 = true; extensions.Remove("glEXT_sparse_texture2"); }
            if (extensions.Contains("gl_EXT_stencil_clear_tag")) { EXT_stencil_clear_tag = true; extensions.Remove("glEXT_stencil_clear_tag"); }
            if (extensions.Contains("gl_EXT_stencil_two_side")) { EXT_stencil_two_side = true; extensions.Remove("glEXT_stencil_two_side"); }
            if (extensions.Contains("gl_EXT_stencil_wrap")) { EXT_stencil_wrap = true; extensions.Remove("glEXT_stencil_wrap"); }
            if (extensions.Contains("gl_EXT_subtexture")) { EXT_subtexture = true; extensions.Remove("glEXT_subtexture"); }
            if (extensions.Contains("gl_EXT_texture")) { EXT_texture = true; extensions.Remove("glEXT_texture"); }
            if (extensions.Contains("gl_EXT_texture3D")) { EXT_texture3D = true; extensions.Remove("glEXT_texture3D"); }
            if (extensions.Contains("gl_EXT_texture_array")) { EXT_texture_array = true; extensions.Remove("glEXT_texture_array"); }
            if (extensions.Contains("gl_EXT_texture_buffer_object")) { EXT_texture_buffer_object = true; extensions.Remove("glEXT_texture_buffer_object"); }
            if (extensions.Contains("gl_EXT_texture_compression_latc")) { EXT_texture_compression_latc = true; extensions.Remove("glEXT_texture_compression_latc"); }
            if (extensions.Contains("gl_EXT_texture_compression_rgtc")) { EXT_texture_compression_rgtc = true; extensions.Remove("glEXT_texture_compression_rgtc"); }
            if (extensions.Contains("gl_EXT_texture_compression_s3tc")) { EXT_texture_compression_s3tc = true; extensions.Remove("glEXT_texture_compression_s3tc"); }
            if (extensions.Contains("gl_EXT_texture_cube_map")) { EXT_texture_cube_map = true; extensions.Remove("glEXT_texture_cube_map"); }
            if (extensions.Contains("gl_EXT_texture_env_add")) { EXT_texture_env_add = true; extensions.Remove("glEXT_texture_env_add"); }
            if (extensions.Contains("gl_EXT_texture_env_combine")) { EXT_texture_env_combine = true; extensions.Remove("glEXT_texture_env_combine"); }
            if (extensions.Contains("gl_EXT_texture_env_dot3")) { EXT_texture_env_dot3 = true; extensions.Remove("glEXT_texture_env_dot3"); }
            if (extensions.Contains("gl_EXT_texture_filter_anisotropic")) { EXT_texture_filter_anisotropic = true; extensions.Remove("glEXT_texture_filter_anisotropic"); }
            if (extensions.Contains("gl_EXT_texture_filter_minmax")) { EXT_texture_filter_minmax = true; extensions.Remove("glEXT_texture_filter_minmax"); }
            if (extensions.Contains("gl_EXT_texture_integer")) { EXT_texture_integer = true; extensions.Remove("glEXT_texture_integer"); }
            if (extensions.Contains("gl_EXT_texture_lod_bias")) { EXT_texture_lod_bias = true; extensions.Remove("glEXT_texture_lod_bias"); }
            if (extensions.Contains("gl_EXT_texture_mirror_clamp")) { EXT_texture_mirror_clamp = true; extensions.Remove("glEXT_texture_mirror_clamp"); }
            if (extensions.Contains("gl_EXT_texture_object")) { EXT_texture_object = true; extensions.Remove("glEXT_texture_object"); }
            if (extensions.Contains("gl_EXT_texture_perturb_normal")) { EXT_texture_perturb_normal = true; extensions.Remove("glEXT_texture_perturb_normal"); }
            if (extensions.Contains("gl_EXT_texture_sRGB")) { EXT_texture_sRGB = true; extensions.Remove("glEXT_texture_sRGB"); }
            if (extensions.Contains("gl_EXT_texture_sRGB_R8")) { EXT_texture_sRGB_R8 = true; extensions.Remove("glEXT_texture_sRGB_R8"); }
            if (extensions.Contains("gl_EXT_texture_sRGB_RG8")) { EXT_texture_sRGB_RG8 = true; extensions.Remove("glEXT_texture_sRGB_RG8"); }
            if (extensions.Contains("gl_EXT_texture_sRGB_decode")) { EXT_texture_sRGB_decode = true; extensions.Remove("glEXT_texture_sRGB_decode"); }
            if (extensions.Contains("gl_EXT_texture_shadow_lod")) { EXT_texture_shadow_lod = true; extensions.Remove("glEXT_texture_shadow_lod"); }
            if (extensions.Contains("gl_EXT_texture_shared_exponent")) { EXT_texture_shared_exponent = true; extensions.Remove("glEXT_texture_shared_exponent"); }
            if (extensions.Contains("gl_EXT_texture_snorm")) { EXT_texture_snorm = true; extensions.Remove("glEXT_texture_snorm"); }
            if (extensions.Contains("gl_EXT_texture_storage")) { EXT_texture_storage = true; extensions.Remove("glEXT_texture_storage"); }
            if (extensions.Contains("gl_EXT_texture_swizzle")) { EXT_texture_swizzle = true; extensions.Remove("glEXT_texture_swizzle"); }
            if (extensions.Contains("gl_EXT_timer_query")) { EXT_timer_query = true; extensions.Remove("glEXT_timer_query"); }
            if (extensions.Contains("gl_EXT_transform_feedback")) { EXT_transform_feedback = true; extensions.Remove("glEXT_transform_feedback"); }
            if (extensions.Contains("gl_EXT_vertex_array")) { EXT_vertex_array = true; extensions.Remove("glEXT_vertex_array"); }
            if (extensions.Contains("gl_EXT_vertex_array_bgra")) { EXT_vertex_array_bgra = true; extensions.Remove("glEXT_vertex_array_bgra"); }
            if (extensions.Contains("gl_EXT_vertex_attrib_64bit")) { EXT_vertex_attrib_64bit = true; extensions.Remove("glEXT_vertex_attrib_64bit"); }
            if (extensions.Contains("gl_EXT_vertex_shader")) { EXT_vertex_shader = true; extensions.Remove("glEXT_vertex_shader"); }
            if (extensions.Contains("gl_EXT_vertex_weighting")) { EXT_vertex_weighting = true; extensions.Remove("glEXT_vertex_weighting"); }
            if (extensions.Contains("gl_EXT_win32_keyed_mutex")) { EXT_win32_keyed_mutex = true; extensions.Remove("glEXT_win32_keyed_mutex"); }
            if (extensions.Contains("gl_EXT_window_rectangles")) { EXT_window_rectangles = true; extensions.Remove("glEXT_window_rectangles"); }
            if (extensions.Contains("gl_EXT_x11_sync_object")) { EXT_x11_sync_object = true; extensions.Remove("glEXT_x11_sync_object"); }
            if (extensions.Contains("gl_GREMEDY_frame_terminator")) { GREMEDY_frame_terminator = true; extensions.Remove("glGREMEDY_frame_terminator"); }
            if (extensions.Contains("gl_GREMEDY_string_marker")) { GREMEDY_string_marker = true; extensions.Remove("glGREMEDY_string_marker"); }
            if (extensions.Contains("gl_HP_convolution_border_modes")) { HP_convolution_border_modes = true; extensions.Remove("glHP_convolution_border_modes"); }
            if (extensions.Contains("gl_HP_image_transform")) { HP_image_transform = true; extensions.Remove("glHP_image_transform"); }
            if (extensions.Contains("gl_HP_occlusion_test")) { HP_occlusion_test = true; extensions.Remove("glHP_occlusion_test"); }
            if (extensions.Contains("gl_HP_texture_lighting")) { HP_texture_lighting = true; extensions.Remove("glHP_texture_lighting"); }
            if (extensions.Contains("gl_IBM_cull_vertex")) { IBM_cull_vertex = true; extensions.Remove("glIBM_cull_vertex"); }
            if (extensions.Contains("gl_IBM_multimode_draw_arrays")) { IBM_multimode_draw_arrays = true; extensions.Remove("glIBM_multimode_draw_arrays"); }
            if (extensions.Contains("gl_IBM_rasterpos_clip")) { IBM_rasterpos_clip = true; extensions.Remove("glIBM_rasterpos_clip"); }
            if (extensions.Contains("gl_IBM_static_data")) { IBM_static_data = true; extensions.Remove("glIBM_static_data"); }
            if (extensions.Contains("gl_IBM_texture_mirrored_repeat")) { IBM_texture_mirrored_repeat = true; extensions.Remove("glIBM_texture_mirrored_repeat"); }
            if (extensions.Contains("gl_IBM_vertex_array_lists")) { IBM_vertex_array_lists = true; extensions.Remove("glIBM_vertex_array_lists"); }
            if (extensions.Contains("gl_INGR_blend_func_separate")) { INGR_blend_func_separate = true; extensions.Remove("glINGR_blend_func_separate"); }
            if (extensions.Contains("gl_INGR_color_clamp")) { INGR_color_clamp = true; extensions.Remove("glINGR_color_clamp"); }
            if (extensions.Contains("gl_INGR_interlace_read")) { INGR_interlace_read = true; extensions.Remove("glINGR_interlace_read"); }
            if (extensions.Contains("gl_INTEL_blackhole_render")) { INTEL_blackhole_render = true; extensions.Remove("glINTEL_blackhole_render"); }
            if (extensions.Contains("gl_INTEL_conservative_rasterization")) { INTEL_conservative_rasterization = true; extensions.Remove("glINTEL_conservative_rasterization"); }
            if (extensions.Contains("gl_INTEL_fragment_shader_ordering")) { INTEL_fragment_shader_ordering = true; extensions.Remove("glINTEL_fragment_shader_ordering"); }
            if (extensions.Contains("gl_INTEL_framebuffer_CMAA")) { INTEL_framebuffer_CMAA = true; extensions.Remove("glINTEL_framebuffer_CMAA"); }
            if (extensions.Contains("gl_INTEL_map_texture")) { INTEL_map_texture = true; extensions.Remove("glINTEL_map_texture"); }
            if (extensions.Contains("gl_INTEL_parallel_arrays")) { INTEL_parallel_arrays = true; extensions.Remove("glINTEL_parallel_arrays"); }
            if (extensions.Contains("gl_INTEL_performance_query")) { INTEL_performance_query = true; extensions.Remove("glINTEL_performance_query"); }
            if (extensions.Contains("gl_KHR_blend_equation_advanced")) { KHR_blend_equation_advanced = true; extensions.Remove("glKHR_blend_equation_advanced"); }
            if (extensions.Contains("gl_KHR_blend_equation_advanced_coherent")) { KHR_blend_equation_advanced_coherent = true; extensions.Remove("glKHR_blend_equation_advanced_coherent"); }
            if (extensions.Contains("gl_KHR_context_flush_control")) { KHR_context_flush_control = true; extensions.Remove("glKHR_context_flush_control"); }
            if (extensions.Contains("gl_KHR_debug")) { KHR_debug = true; extensions.Remove("glKHR_debug"); }
            if (extensions.Contains("gl_KHR_no_error")) { KHR_no_error = true; extensions.Remove("glKHR_no_error"); }
            if (extensions.Contains("gl_KHR_parallel_shader_compile")) { KHR_parallel_shader_compile = true; extensions.Remove("glKHR_parallel_shader_compile"); }
            if (extensions.Contains("gl_KHR_robust_buffer_access_behavior")) { KHR_robust_buffer_access_behavior = true; extensions.Remove("glKHR_robust_buffer_access_behavior"); }
            if (extensions.Contains("gl_KHR_robustness")) { KHR_robustness = true; extensions.Remove("glKHR_robustness"); }
            if (extensions.Contains("gl_KHR_shader_subgroup")) { KHR_shader_subgroup = true; extensions.Remove("glKHR_shader_subgroup"); }
            if (extensions.Contains("gl_KHR_texture_compression_astc_hdr")) { KHR_texture_compression_astc_hdr = true; extensions.Remove("glKHR_texture_compression_astc_hdr"); }
            if (extensions.Contains("gl_KHR_texture_compression_astc_ldr")) { KHR_texture_compression_astc_ldr = true; extensions.Remove("glKHR_texture_compression_astc_ldr"); }
            if (extensions.Contains("gl_KHR_texture_compression_astc_sliced_3d")) { KHR_texture_compression_astc_sliced_3d = true; extensions.Remove("glKHR_texture_compression_astc_sliced_3d"); }
            if (extensions.Contains("gl_MESAX_texture_stack")) { MESAX_texture_stack = true; extensions.Remove("glMESAX_texture_stack"); }
            if (extensions.Contains("gl_MESA_framebuffer_flip_x")) { MESA_framebuffer_flip_x = true; extensions.Remove("glMESA_framebuffer_flip_x"); }
            if (extensions.Contains("gl_MESA_framebuffer_flip_y")) { MESA_framebuffer_flip_y = true; extensions.Remove("glMESA_framebuffer_flip_y"); }
            if (extensions.Contains("gl_MESA_framebuffer_swap_xy")) { MESA_framebuffer_swap_xy = true; extensions.Remove("glMESA_framebuffer_swap_xy"); }
            if (extensions.Contains("gl_MESA_pack_invert")) { MESA_pack_invert = true; extensions.Remove("glMESA_pack_invert"); }
            if (extensions.Contains("gl_MESA_program_binary_formats")) { MESA_program_binary_formats = true; extensions.Remove("glMESA_program_binary_formats"); }
            if (extensions.Contains("gl_MESA_resize_buffers")) { MESA_resize_buffers = true; extensions.Remove("glMESA_resize_buffers"); }
            if (extensions.Contains("gl_MESA_shader_integer_functions")) { MESA_shader_integer_functions = true; extensions.Remove("glMESA_shader_integer_functions"); }
            if (extensions.Contains("gl_MESA_tile_raster_order")) { MESA_tile_raster_order = true; extensions.Remove("glMESA_tile_raster_order"); }
            if (extensions.Contains("gl_MESA_window_pos")) { MESA_window_pos = true; extensions.Remove("glMESA_window_pos"); }
            if (extensions.Contains("gl_MESA_ycbcr_texture")) { MESA_ycbcr_texture = true; extensions.Remove("glMESA_ycbcr_texture"); }
            if (extensions.Contains("gl_NVX_blend_equation_advanced_multi_draw_buffers")) { NVX_blend_equation_advanced_multi_draw_buffers = true; extensions.Remove("glNVX_blend_equation_advanced_multi_draw_buffers"); }
            if (extensions.Contains("gl_NVX_conditional_render")) { NVX_conditional_render = true; extensions.Remove("glNVX_conditional_render"); }
            if (extensions.Contains("gl_NVX_gpu_memory_info")) { NVX_gpu_memory_info = true; extensions.Remove("glNVX_gpu_memory_info"); }
            if (extensions.Contains("gl_NVX_gpu_multicast2")) { NVX_gpu_multicast2 = true; extensions.Remove("glNVX_gpu_multicast2"); }
            if (extensions.Contains("gl_NVX_linked_gpu_multicast")) { NVX_linked_gpu_multicast = true; extensions.Remove("glNVX_linked_gpu_multicast"); }
            if (extensions.Contains("gl_NVX_progress_fence")) { NVX_progress_fence = true; extensions.Remove("glNVX_progress_fence"); }
            if (extensions.Contains("gl_NV_alpha_to_coverage_dither_control")) { NV_alpha_to_coverage_dither_control = true; extensions.Remove("glNV_alpha_to_coverage_dither_control"); }
            if (extensions.Contains("gl_NV_bindless_multi_draw_indirect")) { NV_bindless_multi_draw_indirect = true; extensions.Remove("glNV_bindless_multi_draw_indirect"); }
            if (extensions.Contains("gl_NV_bindless_multi_draw_indirect_count")) { NV_bindless_multi_draw_indirect_count = true; extensions.Remove("glNV_bindless_multi_draw_indirect_count"); }
            if (extensions.Contains("gl_NV_bindless_texture")) { NV_bindless_texture = true; extensions.Remove("glNV_bindless_texture"); }
            if (extensions.Contains("gl_NV_blend_equation_advanced")) { NV_blend_equation_advanced = true; extensions.Remove("glNV_blend_equation_advanced"); }
            if (extensions.Contains("gl_NV_blend_equation_advanced_coherent")) { NV_blend_equation_advanced_coherent = true; extensions.Remove("glNV_blend_equation_advanced_coherent"); }
            if (extensions.Contains("gl_NV_blend_minmax_factor")) { NV_blend_minmax_factor = true; extensions.Remove("glNV_blend_minmax_factor"); }
            if (extensions.Contains("gl_NV_blend_square")) { NV_blend_square = true; extensions.Remove("glNV_blend_square"); }
            if (extensions.Contains("gl_NV_clip_space_w_scaling")) { NV_clip_space_w_scaling = true; extensions.Remove("glNV_clip_space_w_scaling"); }
            if (extensions.Contains("gl_NV_command_list")) { NV_command_list = true; extensions.Remove("glNV_command_list"); }
            if (extensions.Contains("gl_NV_compute_program5")) { NV_compute_program5 = true; extensions.Remove("glNV_compute_program5"); }
            if (extensions.Contains("gl_NV_compute_shader_derivatives")) { NV_compute_shader_derivatives = true; extensions.Remove("glNV_compute_shader_derivatives"); }
            if (extensions.Contains("gl_NV_conditional_render")) { NV_conditional_render = true; extensions.Remove("glNV_conditional_render"); }
            if (extensions.Contains("gl_NV_conservative_raster")) { NV_conservative_raster = true; extensions.Remove("glNV_conservative_raster"); }
            if (extensions.Contains("gl_NV_conservative_raster_dilate")) { NV_conservative_raster_dilate = true; extensions.Remove("glNV_conservative_raster_dilate"); }
            if (extensions.Contains("gl_NV_conservative_raster_pre_snap")) { NV_conservative_raster_pre_snap = true; extensions.Remove("glNV_conservative_raster_pre_snap"); }
            if (extensions.Contains("gl_NV_conservative_raster_pre_snap_triangles")) { NV_conservative_raster_pre_snap_triangles = true; extensions.Remove("glNV_conservative_raster_pre_snap_triangles"); }
            if (extensions.Contains("gl_NV_conservative_raster_underestimation")) { NV_conservative_raster_underestimation = true; extensions.Remove("glNV_conservative_raster_underestimation"); }
            if (extensions.Contains("gl_NV_copy_depth_to_color")) { NV_copy_depth_to_color = true; extensions.Remove("glNV_copy_depth_to_color"); }
            if (extensions.Contains("gl_NV_copy_image")) { NV_copy_image = true; extensions.Remove("glNV_copy_image"); }
            if (extensions.Contains("gl_NV_deep_texture3D")) { NV_deep_texture3D = true; extensions.Remove("glNV_deep_texture3D"); }
            if (extensions.Contains("gl_NV_depth_buffer_float")) { NV_depth_buffer_float = true; extensions.Remove("glNV_depth_buffer_float"); }
            if (extensions.Contains("gl_NV_depth_clamp")) { NV_depth_clamp = true; extensions.Remove("glNV_depth_clamp"); }
            if (extensions.Contains("gl_NV_draw_texture")) { NV_draw_texture = true; extensions.Remove("glNV_draw_texture"); }
            if (extensions.Contains("gl_NV_draw_vulkan_image")) { NV_draw_vulkan_image = true; extensions.Remove("glNV_draw_vulkan_image"); }
            if (extensions.Contains("gl_NV_evaluators")) { NV_evaluators = true; extensions.Remove("glNV_evaluators"); }
            if (extensions.Contains("gl_NV_explicit_multisample")) { NV_explicit_multisample = true; extensions.Remove("glNV_explicit_multisample"); }
            if (extensions.Contains("gl_NV_fence")) { NV_fence = true; extensions.Remove("glNV_fence"); }
            if (extensions.Contains("gl_NV_fill_rectangle")) { NV_fill_rectangle = true; extensions.Remove("glNV_fill_rectangle"); }
            if (extensions.Contains("gl_NV_float_buffer")) { NV_float_buffer = true; extensions.Remove("glNV_float_buffer"); }
            if (extensions.Contains("gl_NV_fog_distance")) { NV_fog_distance = true; extensions.Remove("glNV_fog_distance"); }
            if (extensions.Contains("gl_NV_fragment_coverage_to_color")) { NV_fragment_coverage_to_color = true; extensions.Remove("glNV_fragment_coverage_to_color"); }
            if (extensions.Contains("gl_NV_fragment_program")) { NV_fragment_program = true; extensions.Remove("glNV_fragment_program"); }
            if (extensions.Contains("gl_NV_fragment_program2")) { NV_fragment_program2 = true; extensions.Remove("glNV_fragment_program2"); }
            if (extensions.Contains("gl_NV_fragment_program4")) { NV_fragment_program4 = true; extensions.Remove("glNV_fragment_program4"); }
            if (extensions.Contains("gl_NV_fragment_program_option")) { NV_fragment_program_option = true; extensions.Remove("glNV_fragment_program_option"); }
            if (extensions.Contains("gl_NV_fragment_shader_barycentric")) { NV_fragment_shader_barycentric = true; extensions.Remove("glNV_fragment_shader_barycentric"); }
            if (extensions.Contains("gl_NV_fragment_shader_interlock")) { NV_fragment_shader_interlock = true; extensions.Remove("glNV_fragment_shader_interlock"); }
            if (extensions.Contains("gl_NV_framebuffer_mixed_samples")) { NV_framebuffer_mixed_samples = true; extensions.Remove("glNV_framebuffer_mixed_samples"); }
            if (extensions.Contains("gl_NV_framebuffer_multisample_coverage")) { NV_framebuffer_multisample_coverage = true; extensions.Remove("glNV_framebuffer_multisample_coverage"); }
            if (extensions.Contains("gl_NV_geometry_program4")) { NV_geometry_program4 = true; extensions.Remove("glNV_geometry_program4"); }
            if (extensions.Contains("gl_NV_geometry_shader4")) { NV_geometry_shader4 = true; extensions.Remove("glNV_geometry_shader4"); }
            if (extensions.Contains("gl_NV_geometry_shader_passthrough")) { NV_geometry_shader_passthrough = true; extensions.Remove("glNV_geometry_shader_passthrough"); }
            if (extensions.Contains("gl_NV_gpu_multicast")) { NV_gpu_multicast = true; extensions.Remove("glNV_gpu_multicast"); }
            if (extensions.Contains("gl_NV_gpu_program4")) { NV_gpu_program4 = true; extensions.Remove("glNV_gpu_program4"); }
            if (extensions.Contains("gl_NV_gpu_program5")) { NV_gpu_program5 = true; extensions.Remove("glNV_gpu_program5"); }
            if (extensions.Contains("gl_NV_gpu_program5_mem_extended")) { NV_gpu_program5_mem_extended = true; extensions.Remove("glNV_gpu_program5_mem_extended"); }
            if (extensions.Contains("gl_NV_gpu_shader5")) { NV_gpu_shader5 = true; extensions.Remove("glNV_gpu_shader5"); }
            if (extensions.Contains("gl_NV_half_float")) { NV_half_float = true; extensions.Remove("glNV_half_float"); }
            if (extensions.Contains("gl_NV_internalformat_sample_query")) { NV_internalformat_sample_query = true; extensions.Remove("glNV_internalformat_sample_query"); }
            if (extensions.Contains("gl_NV_light_max_exponent")) { NV_light_max_exponent = true; extensions.Remove("glNV_light_max_exponent"); }
            if (extensions.Contains("gl_NV_memory_attachment")) { NV_memory_attachment = true; extensions.Remove("glNV_memory_attachment"); }
            if (extensions.Contains("gl_NV_memory_object_sparse")) { NV_memory_object_sparse = true; extensions.Remove("glNV_memory_object_sparse"); }
            if (extensions.Contains("gl_NV_mesh_shader")) { NV_mesh_shader = true; extensions.Remove("glNV_mesh_shader"); }
            if (extensions.Contains("gl_NV_multisample_coverage")) { NV_multisample_coverage = true; extensions.Remove("glNV_multisample_coverage"); }
            if (extensions.Contains("gl_NV_multisample_filter_hint")) { NV_multisample_filter_hint = true; extensions.Remove("glNV_multisample_filter_hint"); }
            if (extensions.Contains("gl_NV_occlusion_query")) { NV_occlusion_query = true; extensions.Remove("glNV_occlusion_query"); }
            if (extensions.Contains("gl_NV_packed_depth_stencil")) { NV_packed_depth_stencil = true; extensions.Remove("glNV_packed_depth_stencil"); }
            if (extensions.Contains("gl_NV_parameter_buffer_object")) { NV_parameter_buffer_object = true; extensions.Remove("glNV_parameter_buffer_object"); }
            if (extensions.Contains("gl_NV_parameter_buffer_object2")) { NV_parameter_buffer_object2 = true; extensions.Remove("glNV_parameter_buffer_object2"); }
            if (extensions.Contains("gl_NV_path_rendering")) { NV_path_rendering = true; extensions.Remove("glNV_path_rendering"); }
            if (extensions.Contains("gl_NV_path_rendering_shared_edge")) { NV_path_rendering_shared_edge = true; extensions.Remove("glNV_path_rendering_shared_edge"); }
            if (extensions.Contains("gl_NV_pixel_data_range")) { NV_pixel_data_range = true; extensions.Remove("glNV_pixel_data_range"); }
            if (extensions.Contains("gl_NV_point_sprite")) { NV_point_sprite = true; extensions.Remove("glNV_point_sprite"); }
            if (extensions.Contains("gl_NV_present_video")) { NV_present_video = true; extensions.Remove("glNV_present_video"); }
            if (extensions.Contains("gl_NV_primitive_restart")) { NV_primitive_restart = true; extensions.Remove("glNV_primitive_restart"); }
            if (extensions.Contains("gl_NV_primitive_shading_rate")) { NV_primitive_shading_rate = true; extensions.Remove("glNV_primitive_shading_rate"); }
            if (extensions.Contains("gl_NV_query_resource")) { NV_query_resource = true; extensions.Remove("glNV_query_resource"); }
            if (extensions.Contains("gl_NV_query_resource_tag")) { NV_query_resource_tag = true; extensions.Remove("glNV_query_resource_tag"); }
            if (extensions.Contains("gl_NV_register_combiners")) { NV_register_combiners = true; extensions.Remove("glNV_register_combiners"); }
            if (extensions.Contains("gl_NV_register_combiners2")) { NV_register_combiners2 = true; extensions.Remove("glNV_register_combiners2"); }
            if (extensions.Contains("gl_NV_representative_fragment_test")) { NV_representative_fragment_test = true; extensions.Remove("glNV_representative_fragment_test"); }
            if (extensions.Contains("gl_NV_robustness_video_memory_purge")) { NV_robustness_video_memory_purge = true; extensions.Remove("glNV_robustness_video_memory_purge"); }
            if (extensions.Contains("gl_NV_sample_locations")) { NV_sample_locations = true; extensions.Remove("glNV_sample_locations"); }
            if (extensions.Contains("gl_NV_sample_mask_override_coverage")) { NV_sample_mask_override_coverage = true; extensions.Remove("glNV_sample_mask_override_coverage"); }
            if (extensions.Contains("gl_NV_scissor_exclusive")) { NV_scissor_exclusive = true; extensions.Remove("glNV_scissor_exclusive"); }
            if (extensions.Contains("gl_NV_shader_atomic_counters")) { NV_shader_atomic_counters = true; extensions.Remove("glNV_shader_atomic_counters"); }
            if (extensions.Contains("gl_NV_shader_atomic_float")) { NV_shader_atomic_float = true; extensions.Remove("glNV_shader_atomic_float"); }
            if (extensions.Contains("gl_NV_shader_atomic_float64")) { NV_shader_atomic_float64 = true; extensions.Remove("glNV_shader_atomic_float64"); }
            if (extensions.Contains("gl_NV_shader_atomic_fp16_vector")) { NV_shader_atomic_fp16_vector = true; extensions.Remove("glNV_shader_atomic_fp16_vector"); }
            if (extensions.Contains("gl_NV_shader_atomic_int64")) { NV_shader_atomic_int64 = true; extensions.Remove("glNV_shader_atomic_int64"); }
            if (extensions.Contains("gl_NV_shader_buffer_load")) { NV_shader_buffer_load = true; extensions.Remove("glNV_shader_buffer_load"); }
            if (extensions.Contains("gl_NV_shader_buffer_store")) { NV_shader_buffer_store = true; extensions.Remove("glNV_shader_buffer_store"); }
            if (extensions.Contains("gl_NV_shader_storage_buffer_object")) { NV_shader_storage_buffer_object = true; extensions.Remove("glNV_shader_storage_buffer_object"); }
            if (extensions.Contains("gl_NV_shader_subgroup_partitioned")) { NV_shader_subgroup_partitioned = true; extensions.Remove("glNV_shader_subgroup_partitioned"); }
            if (extensions.Contains("gl_NV_shader_texture_footprint")) { NV_shader_texture_footprint = true; extensions.Remove("glNV_shader_texture_footprint"); }
            if (extensions.Contains("gl_NV_shader_thread_group")) { NV_shader_thread_group = true; extensions.Remove("glNV_shader_thread_group"); }
            if (extensions.Contains("gl_NV_shader_thread_shuffle")) { NV_shader_thread_shuffle = true; extensions.Remove("glNV_shader_thread_shuffle"); }
            if (extensions.Contains("gl_NV_shading_rate_image")) { NV_shading_rate_image = true; extensions.Remove("glNV_shading_rate_image"); }
            if (extensions.Contains("gl_NV_stereo_view_rendering")) { NV_stereo_view_rendering = true; extensions.Remove("glNV_stereo_view_rendering"); }
            if (extensions.Contains("gl_NV_tessellation_program5")) { NV_tessellation_program5 = true; extensions.Remove("glNV_tessellation_program5"); }
            if (extensions.Contains("gl_NV_texgen_emboss")) { NV_texgen_emboss = true; extensions.Remove("glNV_texgen_emboss"); }
            if (extensions.Contains("gl_NV_texgen_reflection")) { NV_texgen_reflection = true; extensions.Remove("glNV_texgen_reflection"); }
            if (extensions.Contains("gl_NV_texture_barrier")) { NV_texture_barrier = true; extensions.Remove("glNV_texture_barrier"); }
            if (extensions.Contains("gl_NV_texture_compression_vtc")) { NV_texture_compression_vtc = true; extensions.Remove("glNV_texture_compression_vtc"); }
            if (extensions.Contains("gl_NV_texture_env_combine4")) { NV_texture_env_combine4 = true; extensions.Remove("glNV_texture_env_combine4"); }
            if (extensions.Contains("gl_NV_texture_expand_normal")) { NV_texture_expand_normal = true; extensions.Remove("glNV_texture_expand_normal"); }
            if (extensions.Contains("gl_NV_texture_multisample")) { NV_texture_multisample = true; extensions.Remove("glNV_texture_multisample"); }
            if (extensions.Contains("gl_NV_texture_rectangle")) { NV_texture_rectangle = true; extensions.Remove("glNV_texture_rectangle"); }
            if (extensions.Contains("gl_NV_texture_rectangle_compressed")) { NV_texture_rectangle_compressed = true; extensions.Remove("glNV_texture_rectangle_compressed"); }
            if (extensions.Contains("gl_NV_texture_shader")) { NV_texture_shader = true; extensions.Remove("glNV_texture_shader"); }
            if (extensions.Contains("gl_NV_texture_shader2")) { NV_texture_shader2 = true; extensions.Remove("glNV_texture_shader2"); }
            if (extensions.Contains("gl_NV_texture_shader3")) { NV_texture_shader3 = true; extensions.Remove("glNV_texture_shader3"); }
            if (extensions.Contains("gl_NV_timeline_semaphore")) { NV_timeline_semaphore = true; extensions.Remove("glNV_timeline_semaphore"); }
            if (extensions.Contains("gl_NV_transform_feedback")) { NV_transform_feedback = true; extensions.Remove("glNV_transform_feedback"); }
            if (extensions.Contains("gl_NV_transform_feedback2")) { NV_transform_feedback2 = true; extensions.Remove("glNV_transform_feedback2"); }
            if (extensions.Contains("gl_NV_uniform_buffer_std430_layout")) { NV_uniform_buffer_std430_layout = true; extensions.Remove("glNV_uniform_buffer_std430_layout"); }
            if (extensions.Contains("gl_NV_uniform_buffer_unified_memory")) { NV_uniform_buffer_unified_memory = true; extensions.Remove("glNV_uniform_buffer_unified_memory"); }
            if (extensions.Contains("gl_NV_vdpau_interop")) { NV_vdpau_interop = true; extensions.Remove("glNV_vdpau_interop"); }
            if (extensions.Contains("gl_NV_vdpau_interop2")) { NV_vdpau_interop2 = true; extensions.Remove("glNV_vdpau_interop2"); }
            if (extensions.Contains("gl_NV_vertex_array_range")) { NV_vertex_array_range = true; extensions.Remove("glNV_vertex_array_range"); }
            if (extensions.Contains("gl_NV_vertex_array_range2")) { NV_vertex_array_range2 = true; extensions.Remove("glNV_vertex_array_range2"); }
            if (extensions.Contains("gl_NV_vertex_attrib_integer_64bit")) { NV_vertex_attrib_integer_64bit = true; extensions.Remove("glNV_vertex_attrib_integer_64bit"); }
            if (extensions.Contains("gl_NV_vertex_buffer_unified_memory")) { NV_vertex_buffer_unified_memory = true; extensions.Remove("glNV_vertex_buffer_unified_memory"); }
            if (extensions.Contains("gl_NV_vertex_program")) { NV_vertex_program = true; extensions.Remove("glNV_vertex_program"); }
            if (extensions.Contains("gl_NV_vertex_program1_1")) { NV_vertex_program1_1 = true; extensions.Remove("glNV_vertex_program1_1"); }
            if (extensions.Contains("gl_NV_vertex_program2")) { NV_vertex_program2 = true; extensions.Remove("glNV_vertex_program2"); }
            if (extensions.Contains("gl_NV_vertex_program2_option")) { NV_vertex_program2_option = true; extensions.Remove("glNV_vertex_program2_option"); }
            if (extensions.Contains("gl_NV_vertex_program3")) { NV_vertex_program3 = true; extensions.Remove("glNV_vertex_program3"); }
            if (extensions.Contains("gl_NV_vertex_program4")) { NV_vertex_program4 = true; extensions.Remove("glNV_vertex_program4"); }
            if (extensions.Contains("gl_NV_video_capture")) { NV_video_capture = true; extensions.Remove("glNV_video_capture"); }
            if (extensions.Contains("gl_NV_viewport_array2")) { NV_viewport_array2 = true; extensions.Remove("glNV_viewport_array2"); }
            if (extensions.Contains("gl_NV_viewport_swizzle")) { NV_viewport_swizzle = true; extensions.Remove("glNV_viewport_swizzle"); }
            if (extensions.Contains("gl_OES_byte_coordinates")) { OES_byte_coordinates = true; extensions.Remove("glOES_byte_coordinates"); }
            if (extensions.Contains("gl_OES_compressed_paletted_texture")) { OES_compressed_paletted_texture = true; extensions.Remove("glOES_compressed_paletted_texture"); }
            if (extensions.Contains("gl_OES_fixed_point")) { OES_fixed_point = true; extensions.Remove("glOES_fixed_point"); }
            if (extensions.Contains("gl_OES_query_matrix")) { OES_query_matrix = true; extensions.Remove("glOES_query_matrix"); }
            if (extensions.Contains("gl_OES_read_format")) { OES_read_format = true; extensions.Remove("glOES_read_format"); }
            if (extensions.Contains("gl_OES_single_precision")) { OES_single_precision = true; extensions.Remove("glOES_single_precision"); }
            if (extensions.Contains("gl_OML_interlace")) { OML_interlace = true; extensions.Remove("glOML_interlace"); }
            if (extensions.Contains("gl_OML_resample")) { OML_resample = true; extensions.Remove("glOML_resample"); }
            if (extensions.Contains("gl_OML_subsample")) { OML_subsample = true; extensions.Remove("glOML_subsample"); }
            if (extensions.Contains("gl_OVR_multiview")) { OVR_multiview = true; extensions.Remove("glOVR_multiview"); }
            if (extensions.Contains("gl_OVR_multiview2")) { OVR_multiview2 = true; extensions.Remove("glOVR_multiview2"); }
            if (extensions.Contains("gl_PGI_misc_hints")) { PGI_misc_hints = true; extensions.Remove("glPGI_misc_hints"); }
            if (extensions.Contains("gl_PGI_vertex_hints")) { PGI_vertex_hints = true; extensions.Remove("glPGI_vertex_hints"); }
            if (extensions.Contains("gl_REND_screen_coordinates")) { REND_screen_coordinates = true; extensions.Remove("glREND_screen_coordinates"); }
            if (extensions.Contains("gl_S3_s3tc")) { S3_s3tc = true; extensions.Remove("glS3_s3tc"); }
            if (extensions.Contains("gl_SGIS_detail_texture")) { SGIS_detail_texture = true; extensions.Remove("glSGIS_detail_texture"); }
            if (extensions.Contains("gl_SGIS_fog_function")) { SGIS_fog_function = true; extensions.Remove("glSGIS_fog_function"); }
            if (extensions.Contains("gl_SGIS_generate_mipmap")) { SGIS_generate_mipmap = true; extensions.Remove("glSGIS_generate_mipmap"); }
            if (extensions.Contains("gl_SGIS_multisample")) { SGIS_multisample = true; extensions.Remove("glSGIS_multisample"); }
            if (extensions.Contains("gl_SGIS_pixel_texture")) { SGIS_pixel_texture = true; extensions.Remove("glSGIS_pixel_texture"); }
            if (extensions.Contains("gl_SGIS_point_line_texgen")) { SGIS_point_line_texgen = true; extensions.Remove("glSGIS_point_line_texgen"); }
            if (extensions.Contains("gl_SGIS_point_parameters")) { SGIS_point_parameters = true; extensions.Remove("glSGIS_point_parameters"); }
            if (extensions.Contains("gl_SGIS_sharpen_texture")) { SGIS_sharpen_texture = true; extensions.Remove("glSGIS_sharpen_texture"); }
            if (extensions.Contains("gl_SGIS_texture4D")) { SGIS_texture4D = true; extensions.Remove("glSGIS_texture4D"); }
            if (extensions.Contains("gl_SGIS_texture_border_clamp")) { SGIS_texture_border_clamp = true; extensions.Remove("glSGIS_texture_border_clamp"); }
            if (extensions.Contains("gl_SGIS_texture_color_mask")) { SGIS_texture_color_mask = true; extensions.Remove("glSGIS_texture_color_mask"); }
            if (extensions.Contains("gl_SGIS_texture_edge_clamp")) { SGIS_texture_edge_clamp = true; extensions.Remove("glSGIS_texture_edge_clamp"); }
            if (extensions.Contains("gl_SGIS_texture_filter4")) { SGIS_texture_filter4 = true; extensions.Remove("glSGIS_texture_filter4"); }
            if (extensions.Contains("gl_SGIS_texture_lod")) { SGIS_texture_lod = true; extensions.Remove("glSGIS_texture_lod"); }
            if (extensions.Contains("gl_SGIS_texture_select")) { SGIS_texture_select = true; extensions.Remove("glSGIS_texture_select"); }
            if (extensions.Contains("gl_SGIX_async")) { SGIX_async = true; extensions.Remove("glSGIX_async"); }
            if (extensions.Contains("gl_SGIX_async_histogram")) { SGIX_async_histogram = true; extensions.Remove("glSGIX_async_histogram"); }
            if (extensions.Contains("gl_SGIX_async_pixel")) { SGIX_async_pixel = true; extensions.Remove("glSGIX_async_pixel"); }
            if (extensions.Contains("gl_SGIX_blend_alpha_minmax")) { SGIX_blend_alpha_minmax = true; extensions.Remove("glSGIX_blend_alpha_minmax"); }
            if (extensions.Contains("gl_SGIX_calligraphic_fragment")) { SGIX_calligraphic_fragment = true; extensions.Remove("glSGIX_calligraphic_fragment"); }
            if (extensions.Contains("gl_SGIX_clipmap")) { SGIX_clipmap = true; extensions.Remove("glSGIX_clipmap"); }
            if (extensions.Contains("gl_SGIX_convolution_accuracy")) { SGIX_convolution_accuracy = true; extensions.Remove("glSGIX_convolution_accuracy"); }
            if (extensions.Contains("gl_SGIX_depth_pass_instrument")) { SGIX_depth_pass_instrument = true; extensions.Remove("glSGIX_depth_pass_instrument"); }
            if (extensions.Contains("gl_SGIX_depth_texture")) { SGIX_depth_texture = true; extensions.Remove("glSGIX_depth_texture"); }
            if (extensions.Contains("gl_SGIX_flush_raster")) { SGIX_flush_raster = true; extensions.Remove("glSGIX_flush_raster"); }
            if (extensions.Contains("gl_SGIX_fog_offset")) { SGIX_fog_offset = true; extensions.Remove("glSGIX_fog_offset"); }
            if (extensions.Contains("gl_SGIX_fragment_lighting")) { SGIX_fragment_lighting = true; extensions.Remove("glSGIX_fragment_lighting"); }
            if (extensions.Contains("gl_SGIX_framezoom")) { SGIX_framezoom = true; extensions.Remove("glSGIX_framezoom"); }
            if (extensions.Contains("gl_SGIX_igloo_interface")) { SGIX_igloo_interface = true; extensions.Remove("glSGIX_igloo_interface"); }
            if (extensions.Contains("gl_SGIX_instruments")) { SGIX_instruments = true; extensions.Remove("glSGIX_instruments"); }
            if (extensions.Contains("gl_SGIX_interlace")) { SGIX_interlace = true; extensions.Remove("glSGIX_interlace"); }
            if (extensions.Contains("gl_SGIX_ir_instrument1")) { SGIX_ir_instrument1 = true; extensions.Remove("glSGIX_ir_instrument1"); }
            if (extensions.Contains("gl_SGIX_list_priority")) { SGIX_list_priority = true; extensions.Remove("glSGIX_list_priority"); }
            if (extensions.Contains("gl_SGIX_pixel_texture")) { SGIX_pixel_texture = true; extensions.Remove("glSGIX_pixel_texture"); }
            if (extensions.Contains("gl_SGIX_pixel_tiles")) { SGIX_pixel_tiles = true; extensions.Remove("glSGIX_pixel_tiles"); }
            if (extensions.Contains("gl_SGIX_polynomial_ffd")) { SGIX_polynomial_ffd = true; extensions.Remove("glSGIX_polynomial_ffd"); }
            if (extensions.Contains("gl_SGIX_reference_plane")) { SGIX_reference_plane = true; extensions.Remove("glSGIX_reference_plane"); }
            if (extensions.Contains("gl_SGIX_resample")) { SGIX_resample = true; extensions.Remove("glSGIX_resample"); }
            if (extensions.Contains("gl_SGIX_scalebias_hint")) { SGIX_scalebias_hint = true; extensions.Remove("glSGIX_scalebias_hint"); }
            if (extensions.Contains("gl_SGIX_shadow")) { SGIX_shadow = true; extensions.Remove("glSGIX_shadow"); }
            if (extensions.Contains("gl_SGIX_shadow_ambient")) { SGIX_shadow_ambient = true; extensions.Remove("glSGIX_shadow_ambient"); }
            if (extensions.Contains("gl_SGIX_sprite")) { SGIX_sprite = true; extensions.Remove("glSGIX_sprite"); }
            if (extensions.Contains("gl_SGIX_subsample")) { SGIX_subsample = true; extensions.Remove("glSGIX_subsample"); }
            if (extensions.Contains("gl_SGIX_tag_sample_buffer")) { SGIX_tag_sample_buffer = true; extensions.Remove("glSGIX_tag_sample_buffer"); }
            if (extensions.Contains("gl_SGIX_texture_add_env")) { SGIX_texture_add_env = true; extensions.Remove("glSGIX_texture_add_env"); }
            if (extensions.Contains("gl_SGIX_texture_coordinate_clamp")) { SGIX_texture_coordinate_clamp = true; extensions.Remove("glSGIX_texture_coordinate_clamp"); }
            if (extensions.Contains("gl_SGIX_texture_lod_bias")) { SGIX_texture_lod_bias = true; extensions.Remove("glSGIX_texture_lod_bias"); }
            if (extensions.Contains("gl_SGIX_texture_multi_buffer")) { SGIX_texture_multi_buffer = true; extensions.Remove("glSGIX_texture_multi_buffer"); }
            if (extensions.Contains("gl_SGIX_texture_scale_bias")) { SGIX_texture_scale_bias = true; extensions.Remove("glSGIX_texture_scale_bias"); }
            if (extensions.Contains("gl_SGIX_vertex_preclip")) { SGIX_vertex_preclip = true; extensions.Remove("glSGIX_vertex_preclip"); }
            if (extensions.Contains("gl_SGIX_ycrcb")) { SGIX_ycrcb = true; extensions.Remove("glSGIX_ycrcb"); }
            if (extensions.Contains("gl_SGIX_ycrcb_subsample")) { SGIX_ycrcb_subsample = true; extensions.Remove("glSGIX_ycrcb_subsample"); }
            if (extensions.Contains("gl_SGIX_ycrcba")) { SGIX_ycrcba = true; extensions.Remove("glSGIX_ycrcba"); }
            if (extensions.Contains("gl_SGI_color_matrix")) { SGI_color_matrix = true; extensions.Remove("glSGI_color_matrix"); }
            if (extensions.Contains("gl_SGI_color_table")) { SGI_color_table = true; extensions.Remove("glSGI_color_table"); }
            if (extensions.Contains("gl_SGI_texture_color_table")) { SGI_texture_color_table = true; extensions.Remove("glSGI_texture_color_table"); }
            if (extensions.Contains("gl_SUNX_constant_data")) { SUNX_constant_data = true; extensions.Remove("glSUNX_constant_data"); }
            if (extensions.Contains("gl_SUN_convolution_border_modes")) { SUN_convolution_border_modes = true; extensions.Remove("glSUN_convolution_border_modes"); }
            if (extensions.Contains("gl_SUN_global_alpha")) { SUN_global_alpha = true; extensions.Remove("glSUN_global_alpha"); }
            if (extensions.Contains("gl_SUN_mesh_array")) { SUN_mesh_array = true; extensions.Remove("glSUN_mesh_array"); }
            if (extensions.Contains("gl_SUN_slice_accum")) { SUN_slice_accum = true; extensions.Remove("glSUN_slice_accum"); }
            if (extensions.Contains("gl_SUN_triangle_list")) { SUN_triangle_list = true; extensions.Remove("glSUN_triangle_list"); }
            if (extensions.Contains("gl_SUN_vertex")) { SUN_vertex = true; extensions.Remove("glSUN_vertex"); }
            if (extensions.Contains("gl_WIN_phong_shading")) { WIN_phong_shading = true; extensions.Remove("glWIN_phong_shading"); }
            if (extensions.Contains("gl_WIN_specular_fog")) { WIN_specular_fog = true; extensions.Remove("glWIN_specular_fog"); }
            #endregion

            InternalIO.InternalWriteLog("Find OpenGL extensions.", LogType.Info);

        }

        /// <summary>
        /// Get the function pointer of given OpenGL api name
        /// </summary>
        /// <param name="name">Name of the OpenGL api</param>
        /// <returns>The funtion pointer of the api in form of <see cref="IntPtr"/></returns>
        public static IntPtr LoadGLfunc(string name)
        {
            IntPtr ret = InternalIO.glfw.GetProcAddress(name);
            if (ret == default)
            {
                InternalIO.InternalAppendLog(
                    $"Cannot load OpenGL api called {name}, this api may not exist or supported.");
                return ret;
            }
            return ret;
        }


        internal static void CheckCoreVersion(int majorVersion, int minorVersion)
        {
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 0)) core10 = true; else goto log;
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 1)) core11 = true; else goto log;
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 2)) core12 = true; else goto log;
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 3)) core13 = true; else goto log;
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 4)) core14 = true; else goto log;
            if ((majorVersion >= 1) || (majorVersion == 1 && minorVersion == 5)) core15 = true; else goto log;
            if ((majorVersion >= 2) || (majorVersion == 2 && minorVersion == 0)) core20 = true; else goto log;
            if ((majorVersion >= 2) || (majorVersion == 2 && minorVersion == 1)) core21 = true; else goto log;
            if ((majorVersion >= 3) || (majorVersion == 3 && minorVersion == 0)) core30 = true; else goto log;
            if ((majorVersion >= 3) || (majorVersion == 3 && minorVersion == 1)) core31 = true; else goto log;
            if ((majorVersion >= 3) || (majorVersion == 3 && minorVersion == 2)) core32 = true; else goto log;
            if ((majorVersion >= 3) || (majorVersion == 3 && minorVersion == 3)) core33 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 0)) core40 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 1)) core41 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 2)) core42 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 3)) core43 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 4)) core44 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 5)) core45 = true; else goto log;
            if ((majorVersion >= 4) || (majorVersion == 4 && minorVersion == 6)) core46 = true; else goto log;

            log:
            {
                InternalIO.InternalWriteLog("Find core OpenGL version.", LogType.Info);
                return;
            }

        }

        internal unsafe static string GetExtensions()
        {
            if (GL.gl == null)
            {
                return "";
            }
            return Marshal.PtrToStringAnsi(
                GL.gl.GetString(glExtension));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T internalLoadAPI<T>(IntPtr ptr) where T : Delegate
        {
            try
            {
                if (ptr == IntPtr.Zero) { return null; }
                T func = Marshal.GetDelegateForFunctionPointer<T>(ptr);
                return func;
            }
            catch (Exception ex)
            {
                InternalIO.InternalWriteLog(
                    $"Failed to convert the api {typeof(T).Name}.\n" + ex.Message, LogType.Warning);
                return default;
            }
        }

        internal static unsafe void LoadGLcore10(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core10)
            {
                InternalIO.InternalAppendLog("The core version 1.0 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BlendFunc = load("glBlendFunc"); context.BlendFunc = internalLoadAPI<glBlendFunc>(contextPtr->BlendFunc);
            contextPtr->Clear = load("glClear"); context.Clear = internalLoadAPI<glClear>(contextPtr->Clear);
            contextPtr->ClearColor = load("glClearColor"); context.ClearColor = internalLoadAPI<glClearColor>(contextPtr->ClearColor);
            contextPtr->ClearDepth = load("glClearDepth"); context.ClearDepth = internalLoadAPI<glClearDepth>(contextPtr->ClearDepth);
            contextPtr->ClearStencil = load("glClearStencil"); context.ClearStencil = internalLoadAPI<glClearStencil>(contextPtr->ClearStencil);
            contextPtr->ColorMask = load("glColorMask"); context.ColorMask = internalLoadAPI<glColorMask>(contextPtr->ColorMask);
            contextPtr->CullFace = load("glCullFace"); context.CullFace = internalLoadAPI<glCullFace>(contextPtr->CullFace);
            contextPtr->DepthFunc = load("glDepthFunc"); context.DepthFunc = internalLoadAPI<glDepthFunc>(contextPtr->DepthFunc);
            contextPtr->DepthMask = load("glDepthMask"); context.DepthMask = internalLoadAPI<glDepthMask>(contextPtr->DepthMask);
            contextPtr->DepthRange = load("glDepthRange"); context.DepthRange = internalLoadAPI<glDepthRange>(contextPtr->DepthRange);
            contextPtr->Disable = load("glDisable"); context.Disable = internalLoadAPI<glDisable>(contextPtr->Disable);
            contextPtr->DrawBuffer = load("glDrawBuffer"); context.DrawBuffer = internalLoadAPI<glDrawBuffer>(contextPtr->DrawBuffer);
            contextPtr->Enable = load("glEnable"); context.Enable = internalLoadAPI<glEnable>(contextPtr->Enable);
            contextPtr->Finish = load("glFinish"); context.Finish = internalLoadAPI<glFinish>(contextPtr->Finish);
            contextPtr->Flush = load("glFlush"); context.Flush = internalLoadAPI<glFlush>(contextPtr->Flush);
            contextPtr->FrontFace = load("glFrontFace"); context.FrontFace = internalLoadAPI<glFrontFace>(contextPtr->FrontFace);
            contextPtr->GetBooleanv = load("glGetBooleanv"); context.GetBooleanv = internalLoadAPI<glGetBooleanv>(contextPtr->GetBooleanv);
            contextPtr->GetDoublev = load("glGetDoublev"); context.GetDoublev = internalLoadAPI<glGetDoublev>(contextPtr->GetDoublev);
            contextPtr->GetError = load("glGetError"); context.GetError = internalLoadAPI<glGetError>(contextPtr->GetError);
            contextPtr->GetFloatv = load("glGetFloatv"); context.GetFloatv = internalLoadAPI<glGetFloatv>(contextPtr->GetFloatv);
            contextPtr->GetIntegerv = load("glGetIntegerv"); context.GetIntegerv = internalLoadAPI<glGetIntegerv>(contextPtr->GetIntegerv);
            contextPtr->GetString = load("glGetString"); context.GetString = internalLoadAPI<glGetString>(contextPtr->GetString);
            contextPtr->GetTexImage = load("glGetTexImage"); context.GetTexImage = internalLoadAPI<glGetTexImage>(contextPtr->GetTexImage);
            contextPtr->GetTexLevelParameterfv = load("glGetTexLevelParameterfv"); context.GetTexLevelParameterfv = internalLoadAPI<glGetTexLevelParameterfv>(contextPtr->GetTexLevelParameterfv);
            contextPtr->GetTexLevelParameteriv = load("glGetTexLevelParameteriv"); context.GetTexLevelParameteriv = internalLoadAPI<glGetTexLevelParameteriv>(contextPtr->GetTexLevelParameteriv);
            contextPtr->GetTexParameterfv = load("glGetTexParameterfv"); context.GetTexParameterfv = internalLoadAPI<glGetTexParameterfv>(contextPtr->GetTexParameterfv);
            contextPtr->GetTexParameteriv = load("glGetTexParameteriv"); context.GetTexParameteriv = internalLoadAPI<glGetTexParameteriv>(contextPtr->GetTexParameteriv);
            contextPtr->Hint = load("glHint"); context.Hint = internalLoadAPI<glHint>(contextPtr->Hint);
            contextPtr->IsEnabled = load("glIsEnabled"); context.IsEnabled = internalLoadAPI<glIsEnabled>(contextPtr->IsEnabled);
            contextPtr->LineWidth = load("glLineWidth"); context.LineWidth = internalLoadAPI<glLineWidth>(contextPtr->LineWidth);
            contextPtr->LogicOp = load("glLogicOp"); context.LogicOp = internalLoadAPI<glLogicOp>(contextPtr->LogicOp);
            contextPtr->PixelStoref = load("glPixelStoref"); context.PixelStoref = internalLoadAPI<glPixelStoref>(contextPtr->PixelStoref);
            contextPtr->PixelStorei = load("glPixelStorei"); context.PixelStorei = internalLoadAPI<glPixelStorei>(contextPtr->PixelStorei);
            contextPtr->PointSize = load("glPointSize"); context.PointSize = internalLoadAPI<glPointSize>(contextPtr->PointSize);
            contextPtr->PolygonMode = load("glPolygonMode"); context.PolygonMode = internalLoadAPI<glPolygonMode>(contextPtr->PolygonMode);
            contextPtr->ReadBuffer = load("glReadBuffer"); context.ReadBuffer = internalLoadAPI<glReadBuffer>(contextPtr->ReadBuffer);
            contextPtr->ReadPixels = load("glReadPixels"); context.ReadPixels = internalLoadAPI<glReadPixels>(contextPtr->ReadPixels);
            contextPtr->Scissor = load("glScissor"); context.Scissor = internalLoadAPI<glScissor>(contextPtr->Scissor);
            contextPtr->StencilFunc = load("glStencilFunc"); context.StencilFunc = internalLoadAPI<glStencilFunc>(contextPtr->StencilFunc);
            contextPtr->StencilMask = load("glStencilMask"); context.StencilMask = internalLoadAPI<glStencilMask>(contextPtr->StencilMask);
            contextPtr->StencilOp = load("glStencilOp"); context.StencilOp = internalLoadAPI<glStencilOp>(contextPtr->StencilOp);
            contextPtr->TexImage1D = load("glTexImage1D"); context.TexImage1D = internalLoadAPI<glTexImage1D>(contextPtr->TexImage1D);
            contextPtr->TexImage2D = load("glTexImage2D"); context.TexImage2D = internalLoadAPI<glTexImage2D>(contextPtr->TexImage2D);
            contextPtr->TexParameterf = load("glTexParameterf"); context.TexParameterf = internalLoadAPI<glTexParameterf>(contextPtr->TexParameterf);
            contextPtr->TexParameterfv = load("glTexParameterfv"); context.TexParameterfv = internalLoadAPI<glTexParameterfv>(contextPtr->TexParameterfv);
            contextPtr->TexParameteri = load("glTexParameteri"); context.TexParameteri = internalLoadAPI<glTexParameteri>(contextPtr->TexParameteri);
            contextPtr->TexParameteriv = load("glTexParameteriv"); context.TexParameteriv = internalLoadAPI<glTexParameteriv>(contextPtr->TexParameteriv);
            contextPtr->Viewport = load("glViewport"); context.Viewport = internalLoadAPI<glViewport>(contextPtr->Viewport);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.0 APIs");
        }

        internal static unsafe void LoadGLcore11(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core11)
            {
                InternalIO.InternalAppendLog("The core version 1.1 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindTexture = load("glBindTexture"); context.BindTexture = internalLoadAPI<glBindTexture>(contextPtr->BindTexture);
            contextPtr->CopyTexImage1D = load("glCopyTexImage1D"); context.CopyTexImage1D = internalLoadAPI<glCopyTexImage1D>(contextPtr->CopyTexImage1D);
            contextPtr->CopyTexImage2D = load("glCopyTexImage2D"); context.CopyTexImage2D = internalLoadAPI<glCopyTexImage2D>(contextPtr->CopyTexImage2D);
            contextPtr->CopyTexSubImage1D = load("glCopyTexSubImage1D"); context.CopyTexSubImage1D = internalLoadAPI<glCopyTexSubImage1D>(contextPtr->CopyTexSubImage1D);
            contextPtr->CopyTexSubImage2D = load("glCopyTexSubImage2D"); context.CopyTexSubImage2D = internalLoadAPI<glCopyTexSubImage2D>(contextPtr->CopyTexSubImage2D);
            contextPtr->DeleteTextures = load("glDeleteTextures"); context.DeleteTextures = internalLoadAPI<glDeleteTextures>(contextPtr->DeleteTextures);
            contextPtr->DrawArrays = load("glDrawArrays"); context.DrawArrays = internalLoadAPI<glDrawArrays>(contextPtr->DrawArrays);
            contextPtr->DrawElements = load("glDrawElements"); context.DrawElements = internalLoadAPI<glDrawElements>(contextPtr->DrawElements);
            contextPtr->GenTextures = load("glGenTextures"); context.GenTextures = internalLoadAPI<glGenTextures>(contextPtr->GenTextures);
            contextPtr->GetPointerv = load("glGetPointerv"); context.GetPointerv = internalLoadAPI<glGetPointerv>(contextPtr->GetPointerv);
            contextPtr->IsTexture = load("glIsTexture"); context.IsTexture = internalLoadAPI<glIsTexture>(contextPtr->IsTexture);
            contextPtr->PolygonOffset = load("glPolygonOffset"); context.PolygonOffset = internalLoadAPI<glPolygonOffset>(contextPtr->PolygonOffset);
            contextPtr->TexSubImage1D = load("glTexSubImage1D"); context.TexSubImage1D = internalLoadAPI<glTexSubImage1D>(contextPtr->TexSubImage1D);
            contextPtr->TexSubImage2D = load("glTexSubImage2D"); context.TexSubImage2D = internalLoadAPI<glTexSubImage2D>(contextPtr->TexSubImage2D);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.1 APIs");

        }

        internal static unsafe void LoadGLcore12(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core12)
            {
                InternalIO.InternalAppendLog("The core version 1.2 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->CopyTexSubImage3D = load("glCopyTexSubImage3D"); context.CopyTexSubImage3D = internalLoadAPI<glCopyTexSubImage3D>(contextPtr->CopyTexSubImage3D);
            contextPtr->DrawRangeElements = load("glDrawRangeElements"); context.DrawRangeElements = internalLoadAPI<glDrawRangeElements>(contextPtr->DrawRangeElements);
            contextPtr->TexImage3D = load("glTexImage3D"); context.TexImage3D = internalLoadAPI<glTexImage3D>(contextPtr->TexImage3D);
            contextPtr->TexSubImage3D = load("glTexSubImage3D"); context.TexSubImage3D = internalLoadAPI<glTexSubImage3D>(contextPtr->TexSubImage3D);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.2 APIs");
        }

        internal static unsafe void LoadGLcore13(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {

            if (!core13)
            {
                InternalIO.InternalAppendLog("The core version 1.3 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->ActiveTexture = load("glActiveTexture"); context.ActiveTexture = internalLoadAPI<glActiveTexture>(contextPtr->ActiveTexture);
            contextPtr->CompressedTexImage1D = load("glCompressedTexImage1D"); context.CompressedTexImage1D = internalLoadAPI<glCompressedTexImage1D>(contextPtr->CompressedTexImage1D);
            contextPtr->CompressedTexImage2D = load("glCompressedTexImage2D"); context.CompressedTexImage2D = internalLoadAPI<glCompressedTexImage2D>(contextPtr->CompressedTexImage2D);
            contextPtr->CompressedTexImage3D = load("glCompressedTexImage3D"); context.CompressedTexImage3D = internalLoadAPI<glCompressedTexImage3D>(contextPtr->CompressedTexImage3D);
            contextPtr->CompressedTexSubImage1D = load("glCompressedTexSubImage1D"); context.CompressedTexSubImage1D = internalLoadAPI<glCompressedTexSubImage1D>(contextPtr->CompressedTexSubImage1D);
            contextPtr->CompressedTexSubImage2D = load("glCompressedTexSubImage2D"); context.CompressedTexSubImage2D = internalLoadAPI<glCompressedTexSubImage2D>(contextPtr->CompressedTexSubImage2D);
            contextPtr->CompressedTexSubImage3D = load("glCompressedTexSubImage3D"); context.CompressedTexSubImage3D = internalLoadAPI<glCompressedTexSubImage3D>(contextPtr->CompressedTexSubImage3D);
            contextPtr->GetCompressedTexImage = load("glGetCompressedTexImage"); context.GetCompressedTexImage = internalLoadAPI<glGetCompressedTexImage>(contextPtr->GetCompressedTexImage);
            contextPtr->SampleCoverage = load("glSampleCoverage"); context.SampleCoverage = internalLoadAPI<glSampleCoverage>(contextPtr->SampleCoverage);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.3 APIs");

        }

        internal static unsafe void LoadGLcore14(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core14)
            {
                InternalIO.InternalAppendLog("The core version 1.4 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BlendColor = load("glBlendColor"); context.BlendColor = internalLoadAPI<glBlendColor>(contextPtr->BlendColor);
            contextPtr->BlendEquation = load("glBlendEquation"); context.BlendEquation = internalLoadAPI<glBlendEquation>(contextPtr->BlendEquation);
            contextPtr->BlendFuncSeparate = load("glBlendFuncSeparate"); context.BlendFuncSeparate = internalLoadAPI<glBlendFuncSeparate>(contextPtr->BlendFuncSeparate);
            contextPtr->MultiDrawArrays = load("glMultiDrawArrays"); context.MultiDrawArrays = internalLoadAPI<glMultiDrawArrays>(contextPtr->MultiDrawArrays);
            contextPtr->MultiDrawElements = load("glMultiDrawElements"); context.MultiDrawElements = internalLoadAPI<glMultiDrawElements>(contextPtr->MultiDrawElements);
            contextPtr->PointParameterf = load("glPointParameterf"); context.PointParameterf = internalLoadAPI<glPointParameterf>(contextPtr->PointParameterf);
            contextPtr->PointParameterfv = load("glPointParameterfv"); context.PointParameterfv = internalLoadAPI<glPointParameterfv>(contextPtr->PointParameterfv);
            contextPtr->PointParameteri = load("glPointParameteri"); context.PointParameteri = internalLoadAPI<glPointParameteri>(contextPtr->PointParameteri);
            contextPtr->PointParameteriv = load("glPointParameteriv"); context.PointParameteriv = internalLoadAPI<glPointParameteriv>(contextPtr->PointParameteriv);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.4 APIs");

        }

        internal static unsafe void LoadGLcore15(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {

            if (!core15)
            {
                InternalIO.InternalAppendLog("The core version 1.5 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BeginQuery = load("glBeginQuery"); context.BeginQuery = internalLoadAPI<glBeginQuery>(contextPtr->BeginQuery);
            contextPtr->BindBuffer = load("glBindBuffer"); context.BindBuffer = internalLoadAPI<glBindBuffer>(contextPtr->BindBuffer);
            contextPtr->BufferData = load("glBufferData"); context.BufferData = internalLoadAPI<glBufferData>(contextPtr->BufferData);
            contextPtr->BufferSubData = load("glBufferSubData"); context.BufferSubData = internalLoadAPI<glBufferSubData>(contextPtr->BufferSubData);
            contextPtr->DeleteBuffers = load("glDeleteBuffers"); context.DeleteBuffers = internalLoadAPI<glDeleteBuffers>(contextPtr->DeleteBuffers);
            contextPtr->DeleteQueries = load("glDeleteQueries"); context.DeleteQueries = internalLoadAPI<glDeleteQueries>(contextPtr->DeleteQueries);
            contextPtr->EndQuery = load("glEndQuery"); context.EndQuery = internalLoadAPI<glEndQuery>(contextPtr->EndQuery);
            contextPtr->GenBuffers = load("glGenBuffers"); context.GenBuffers = internalLoadAPI<glGenBuffers>(contextPtr->GenBuffers);
            contextPtr->GenQueries = load("glGenQueries"); context.GenQueries = internalLoadAPI<glGenQueries>(contextPtr->GenQueries);
            contextPtr->GetBufferParameteriv = load("glGetBufferParameteriv"); context.GetBufferParameteriv = internalLoadAPI<glGetBufferParameteriv>(contextPtr->GetBufferParameteriv);
            contextPtr->GetBufferPointerv = load("glGetBufferPointerv"); context.GetBufferPointerv = internalLoadAPI<glGetBufferPointerv>(contextPtr->GetBufferPointerv);
            contextPtr->GetBufferSubData = load("glGetBufferSubData"); context.GetBufferSubData = internalLoadAPI<glGetBufferSubData>(contextPtr->GetBufferSubData);
            contextPtr->GetQueryObjectiv = load("glGetQueryObjectiv"); context.GetQueryObjectiv = internalLoadAPI<glGetQueryObjectiv>(contextPtr->GetQueryObjectiv);
            contextPtr->GetQueryObjectuiv = load("glGetQueryObjectuiv"); context.GetQueryObjectuiv = internalLoadAPI<glGetQueryObjectuiv>(contextPtr->GetQueryObjectuiv);
            contextPtr->GetQueryiv = load("glGetQueryiv"); context.GetQueryiv = internalLoadAPI<glGetQueryiv>(contextPtr->GetQueryiv);
            contextPtr->IsBuffer = load("glIsBuffer"); context.IsBuffer = internalLoadAPI<glIsBuffer>(contextPtr->IsBuffer);
            contextPtr->IsQuery = load("glIsQuery"); context.IsQuery = internalLoadAPI<glIsQuery>(contextPtr->IsQuery);
            contextPtr->MapBuffer = load("glMapBuffer"); context.MapBuffer = internalLoadAPI<glMapBuffer>(contextPtr->MapBuffer);
            contextPtr->UnmapBuffer = load("glUnmapBuffer"); context.UnmapBuffer = internalLoadAPI<glUnmapBuffer>(contextPtr->UnmapBuffer);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.5 APIs");

        }

        internal static unsafe void LoadGLcore20(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core20)
            {
                InternalIO.InternalAppendLog("The core version 2.0 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->AttachShader = load("glAttachShader"); context.AttachShader = internalLoadAPI<glAttachShader>(contextPtr->AttachShader);
            contextPtr->BindAttribLocation = load("glBindAttribLocation"); context.BindAttribLocation = internalLoadAPI<glBindAttribLocation>(contextPtr->BindAttribLocation);
            contextPtr->BlendEquationSeparate = load("glBlendEquationSeparate"); context.BlendEquationSeparate = internalLoadAPI<glBlendEquationSeparate>(contextPtr->BlendEquationSeparate);
            contextPtr->CompileShader = load("glCompileShader"); context.CompileShader = internalLoadAPI<glCompileShader>(contextPtr->CompileShader);
            contextPtr->CreateProgram = load("glCreateProgram"); context.CreateProgram = internalLoadAPI<glCreateProgram>(contextPtr->CreateProgram);
            contextPtr->CreateShader = load("glCreateShader"); context.CreateShader = internalLoadAPI<glCreateShader>(contextPtr->CreateShader);
            contextPtr->DeleteProgram = load("glDeleteProgram"); context.DeleteProgram = internalLoadAPI<glDeleteProgram>(contextPtr->DeleteProgram);
            contextPtr->DeleteShader = load("glDeleteShader"); context.DeleteShader = internalLoadAPI<glDeleteShader>(contextPtr->DeleteShader);
            contextPtr->DetachShader = load("glDetachShader"); context.DetachShader = internalLoadAPI<glDetachShader>(contextPtr->DetachShader);
            contextPtr->DisableVertexAttribArray = load("glDisableVertexAttribArray"); context.DisableVertexAttribArray = internalLoadAPI<glDisableVertexAttribArray>(contextPtr->DisableVertexAttribArray);
            contextPtr->DrawBuffers = load("glDrawBuffers"); context.DrawBuffers = internalLoadAPI<glDrawBuffers>(contextPtr->DrawBuffers);
            contextPtr->EnableVertexAttribArray = load("glEnableVertexAttribArray"); context.EnableVertexAttribArray = internalLoadAPI<glEnableVertexAttribArray>(contextPtr->EnableVertexAttribArray);
            contextPtr->GetActiveAttrib = load("glGetActiveAttrib"); context.GetActiveAttrib = internalLoadAPI<glGetActiveAttrib>(contextPtr->GetActiveAttrib);
            contextPtr->GetActiveUniform = load("glGetActiveUniform"); context.GetActiveUniform = internalLoadAPI<glGetActiveUniform>(contextPtr->GetActiveUniform);
            contextPtr->GetAttachedShaders = load("glGetAttachedShaders"); context.GetAttachedShaders = internalLoadAPI<glGetAttachedShaders>(contextPtr->GetAttachedShaders);
            contextPtr->GetAttribLocation = load("glGetAttribLocation"); context.GetAttribLocation = internalLoadAPI<glGetAttribLocation>(contextPtr->GetAttribLocation);
            contextPtr->GetProgramInfoLog = load("glGetProgramInfoLog"); context.GetProgramInfoLog = internalLoadAPI<glGetProgramInfoLog>(contextPtr->GetProgramInfoLog);
            contextPtr->GetProgramiv = load("glGetProgramiv"); context.GetProgramiv = internalLoadAPI<glGetProgramiv>(contextPtr->GetProgramiv);
            contextPtr->GetShaderInfoLog = load("glGetShaderInfoLog"); context.GetShaderInfoLog = internalLoadAPI<glGetShaderInfoLog>(contextPtr->GetShaderInfoLog);
            contextPtr->GetShaderSource = load("glGetShaderSource"); context.GetShaderSource = internalLoadAPI<glGetShaderSource>(contextPtr->GetShaderSource);
            contextPtr->GetShaderiv = load("glGetShaderiv"); context.GetShaderiv = internalLoadAPI<glGetShaderiv>(contextPtr->GetShaderiv);
            contextPtr->GetUniformLocation = load("glGetUniformLocation"); context.GetUniformLocation = internalLoadAPI<glGetUniformLocation>(contextPtr->GetUniformLocation);
            contextPtr->GetUniformfv = load("glGetUniformfv"); context.GetUniformfv = internalLoadAPI<glGetUniformfv>(contextPtr->GetUniformfv);
            contextPtr->GetUniformiv = load("glGetUniformiv"); context.GetUniformiv = internalLoadAPI<glGetUniformiv>(contextPtr->GetUniformiv);
            contextPtr->GetVertexAttribPointerv = load("glGetVertexAttribPointerv"); context.GetVertexAttribPointerv = internalLoadAPI<glGetVertexAttribPointerv>(contextPtr->GetVertexAttribPointerv);
            contextPtr->GetVertexAttribdv = load("glGetVertexAttribdv"); context.GetVertexAttribdv = internalLoadAPI<glGetVertexAttribdv>(contextPtr->GetVertexAttribdv);
            contextPtr->GetVertexAttribfv = load("glGetVertexAttribfv"); context.GetVertexAttribfv = internalLoadAPI<glGetVertexAttribfv>(contextPtr->GetVertexAttribfv);
            contextPtr->GetVertexAttribiv = load("glGetVertexAttribiv"); context.GetVertexAttribiv = internalLoadAPI<glGetVertexAttribiv>(contextPtr->GetVertexAttribiv);
            contextPtr->IsProgram = load("glIsProgram"); context.IsProgram = internalLoadAPI<glIsProgram>(contextPtr->IsProgram);
            contextPtr->IsShader = load("glIsShader"); context.IsShader = internalLoadAPI<glIsShader>(contextPtr->IsShader);
            contextPtr->LinkProgram = load("glLinkProgram"); context.LinkProgram = internalLoadAPI<glLinkProgram>(contextPtr->LinkProgram);
            contextPtr->ShaderSource = load("glShaderSource"); context.ShaderSource = internalLoadAPI<glShaderSource>(contextPtr->ShaderSource);
            contextPtr->StencilFuncSeparate = load("glStencilFuncSeparate"); context.StencilFuncSeparate = internalLoadAPI<glStencilFuncSeparate>(contextPtr->StencilFuncSeparate);
            contextPtr->StencilMaskSeparate = load("glStencilMaskSeparate"); context.StencilMaskSeparate = internalLoadAPI<glStencilMaskSeparate>(contextPtr->StencilMaskSeparate);
            contextPtr->StencilOpSeparate = load("glStencilOpSeparate"); context.StencilOpSeparate = internalLoadAPI<glStencilOpSeparate>(contextPtr->StencilOpSeparate);
            contextPtr->Uniform1f = load("glUniform1f"); context.Uniform1f = internalLoadAPI<glUniform1f>(contextPtr->Uniform1f);
            contextPtr->Uniform1fv = load("glUniform1fv"); context.Uniform1fv = internalLoadAPI<glUniform1fv>(contextPtr->Uniform1fv);
            contextPtr->Uniform1i = load("glUniform1i"); context.Uniform1i = internalLoadAPI<glUniform1i>(contextPtr->Uniform1i);
            contextPtr->Uniform1iv = load("glUniform1iv"); context.Uniform1iv = internalLoadAPI<glUniform1iv>(contextPtr->Uniform1iv);
            contextPtr->Uniform2f = load("glUniform2f"); context.Uniform2f = internalLoadAPI<glUniform2f>(contextPtr->Uniform2f);
            contextPtr->Uniform2fv = load("glUniform2fv"); context.Uniform2fv = internalLoadAPI<glUniform2fv>(contextPtr->Uniform2fv);
            contextPtr->Uniform2i = load("glUniform2i"); context.Uniform2i = internalLoadAPI<glUniform2i>(contextPtr->Uniform2i);
            contextPtr->Uniform2iv = load("glUniform2iv"); context.Uniform2iv = internalLoadAPI<glUniform2iv>(contextPtr->Uniform2iv);
            contextPtr->Uniform3f = load("glUniform3f"); context.Uniform3f = internalLoadAPI<glUniform3f>(contextPtr->Uniform3f);
            contextPtr->Uniform3fv = load("glUniform3fv"); context.Uniform3fv = internalLoadAPI<glUniform3fv>(contextPtr->Uniform3fv);
            contextPtr->Uniform3i = load("glUniform3i"); context.Uniform3i = internalLoadAPI<glUniform3i>(contextPtr->Uniform3i);
            contextPtr->Uniform3iv = load("glUniform3iv"); context.Uniform3iv = internalLoadAPI<glUniform3iv>(contextPtr->Uniform3iv);
            contextPtr->Uniform4f = load("glUniform4f"); context.Uniform4f = internalLoadAPI<glUniform4f>(contextPtr->Uniform4f);
            contextPtr->Uniform4fv = load("glUniform4fv"); context.Uniform4fv = internalLoadAPI<glUniform4fv>(contextPtr->Uniform4fv);
            contextPtr->Uniform4i = load("glUniform4i"); context.Uniform4i = internalLoadAPI<glUniform4i>(contextPtr->Uniform4i);
            contextPtr->Uniform4iv = load("glUniform4iv"); context.Uniform4iv = internalLoadAPI<glUniform4iv>(contextPtr->Uniform4iv);
            contextPtr->UniformMatrix2fv = load("glUniformMatrix2fv"); context.UniformMatrix2fv = internalLoadAPI<glUniformMatrix2fv>(contextPtr->UniformMatrix2fv);
            contextPtr->UniformMatrix3fv = load("glUniformMatrix3fv"); context.UniformMatrix3fv = internalLoadAPI<glUniformMatrix3fv>(contextPtr->UniformMatrix3fv);
            contextPtr->UniformMatrix4fv = load("glUniformMatrix4fv"); context.UniformMatrix4fv = internalLoadAPI<glUniformMatrix4fv>(contextPtr->UniformMatrix4fv);
            contextPtr->UseProgram = load("glUseProgram"); context.UseProgram = internalLoadAPI<glUseProgram>(contextPtr->UseProgram);
            contextPtr->ValidateProgram = load("glValidateProgram"); context.ValidateProgram = internalLoadAPI<glValidateProgram>(contextPtr->ValidateProgram);
            contextPtr->VertexAttrib1d = load("glVertexAttrib1d"); context.VertexAttrib1d = internalLoadAPI<glVertexAttrib1d>(contextPtr->VertexAttrib1d);
            contextPtr->VertexAttrib1dv = load("glVertexAttrib1dv"); context.VertexAttrib1dv = internalLoadAPI<glVertexAttrib1dv>(contextPtr->VertexAttrib1dv);
            contextPtr->VertexAttrib1f = load("glVertexAttrib1f"); context.VertexAttrib1f = internalLoadAPI<glVertexAttrib1f>(contextPtr->VertexAttrib1f);
            contextPtr->VertexAttrib1fv = load("glVertexAttrib1fv"); context.VertexAttrib1fv = internalLoadAPI<glVertexAttrib1fv>(contextPtr->VertexAttrib1fv);
            contextPtr->VertexAttrib1s = load("glVertexAttrib1s"); context.VertexAttrib1s = internalLoadAPI<glVertexAttrib1s>(contextPtr->VertexAttrib1s);
            contextPtr->VertexAttrib1sv = load("glVertexAttrib1sv"); context.VertexAttrib1sv = internalLoadAPI<glVertexAttrib1sv>(contextPtr->VertexAttrib1sv);
            contextPtr->VertexAttrib2d = load("glVertexAttrib2d"); context.VertexAttrib2d = internalLoadAPI<glVertexAttrib2d>(contextPtr->VertexAttrib2d);
            contextPtr->VertexAttrib2dv = load("glVertexAttrib2dv"); context.VertexAttrib2dv = internalLoadAPI<glVertexAttrib2dv>(contextPtr->VertexAttrib2dv);
            contextPtr->VertexAttrib2f = load("glVertexAttrib2f"); context.VertexAttrib2f = internalLoadAPI<glVertexAttrib2f>(contextPtr->VertexAttrib2f);
            contextPtr->VertexAttrib2fv = load("glVertexAttrib2fv"); context.VertexAttrib2fv = internalLoadAPI<glVertexAttrib2fv>(contextPtr->VertexAttrib2fv);
            contextPtr->VertexAttrib2s = load("glVertexAttrib2s"); context.VertexAttrib2s = internalLoadAPI<glVertexAttrib2s>(contextPtr->VertexAttrib2s);
            contextPtr->VertexAttrib2sv = load("glVertexAttrib2sv"); context.VertexAttrib2sv = internalLoadAPI<glVertexAttrib2sv>(contextPtr->VertexAttrib2sv);
            contextPtr->VertexAttrib3d = load("glVertexAttrib3d"); context.VertexAttrib3d = internalLoadAPI<glVertexAttrib3d>(contextPtr->VertexAttrib3d);
            contextPtr->VertexAttrib3dv = load("glVertexAttrib3dv"); context.VertexAttrib3dv = internalLoadAPI<glVertexAttrib3dv>(contextPtr->VertexAttrib3dv);
            contextPtr->VertexAttrib3f = load("glVertexAttrib3f"); context.VertexAttrib3f = internalLoadAPI<glVertexAttrib3f>(contextPtr->VertexAttrib3f);
            contextPtr->VertexAttrib3fv = load("glVertexAttrib3fv"); context.VertexAttrib3fv = internalLoadAPI<glVertexAttrib3fv>(contextPtr->VertexAttrib3fv);
            contextPtr->VertexAttrib3s = load("glVertexAttrib3s"); context.VertexAttrib3s = internalLoadAPI<glVertexAttrib3s>(contextPtr->VertexAttrib3s);
            contextPtr->VertexAttrib3sv = load("glVertexAttrib3sv"); context.VertexAttrib3sv = internalLoadAPI<glVertexAttrib3sv>(contextPtr->VertexAttrib3sv);
            contextPtr->VertexAttrib4Nbv = load("glVertexAttrib4Nbv"); context.VertexAttrib4Nbv = internalLoadAPI<glVertexAttrib4Nbv>(contextPtr->VertexAttrib4Nbv);
            contextPtr->VertexAttrib4Niv = load("glVertexAttrib4Niv"); context.VertexAttrib4Niv = internalLoadAPI<glVertexAttrib4Niv>(contextPtr->VertexAttrib4Niv);
            contextPtr->VertexAttrib4Nsv = load("glVertexAttrib4Nsv"); context.VertexAttrib4Nsv = internalLoadAPI<glVertexAttrib4Nsv>(contextPtr->VertexAttrib4Nsv);
            contextPtr->VertexAttrib4Nub = load("glVertexAttrib4Nub"); context.VertexAttrib4Nub = internalLoadAPI<glVertexAttrib4Nub>(contextPtr->VertexAttrib4Nub);
            contextPtr->VertexAttrib4Nubv = load("glVertexAttrib4Nubv"); context.VertexAttrib4Nubv = internalLoadAPI<glVertexAttrib4Nubv>(contextPtr->VertexAttrib4Nubv);
            contextPtr->VertexAttrib4Nuiv = load("glVertexAttrib4Nuiv"); context.VertexAttrib4Nuiv = internalLoadAPI<glVertexAttrib4Nuiv>(contextPtr->VertexAttrib4Nuiv);
            contextPtr->VertexAttrib4Nusv = load("glVertexAttrib4Nusv"); context.VertexAttrib4Nusv = internalLoadAPI<glVertexAttrib4Nusv>(contextPtr->VertexAttrib4Nusv);
            contextPtr->VertexAttrib4bv = load("glVertexAttrib4bv"); context.VertexAttrib4bv = internalLoadAPI<glVertexAttrib4bv>(contextPtr->VertexAttrib4bv);
            contextPtr->VertexAttrib4d = load("glVertexAttrib4d"); context.VertexAttrib4d = internalLoadAPI<glVertexAttrib4d>(contextPtr->VertexAttrib4d);
            contextPtr->VertexAttrib4dv = load("glVertexAttrib4dv"); context.VertexAttrib4dv = internalLoadAPI<glVertexAttrib4dv>(contextPtr->VertexAttrib4dv);
            contextPtr->VertexAttrib4f = load("glVertexAttrib4f"); context.VertexAttrib4f = internalLoadAPI<glVertexAttrib4f>(contextPtr->VertexAttrib4f);
            contextPtr->VertexAttrib4fv = load("glVertexAttrib4fv"); context.VertexAttrib4fv = internalLoadAPI<glVertexAttrib4fv>(contextPtr->VertexAttrib4fv);
            contextPtr->VertexAttrib4iv = load("glVertexAttrib4iv"); context.VertexAttrib4iv = internalLoadAPI<glVertexAttrib4iv>(contextPtr->VertexAttrib4iv);
            contextPtr->VertexAttrib4s = load("glVertexAttrib4s"); context.VertexAttrib4s = internalLoadAPI<glVertexAttrib4s>(contextPtr->VertexAttrib4s);
            contextPtr->VertexAttrib4sv = load("glVertexAttrib4sv"); context.VertexAttrib4sv = internalLoadAPI<glVertexAttrib4sv>(contextPtr->VertexAttrib4sv);
            contextPtr->VertexAttrib4ubv = load("glVertexAttrib4ubv"); context.VertexAttrib4ubv = internalLoadAPI<glVertexAttrib4ubv>(contextPtr->VertexAttrib4ubv);
            contextPtr->VertexAttrib4uiv = load("glVertexAttrib4uiv"); context.VertexAttrib4uiv = internalLoadAPI<glVertexAttrib4uiv>(contextPtr->VertexAttrib4uiv);
            contextPtr->VertexAttrib4usv = load("glVertexAttrib4usv"); context.VertexAttrib4usv = internalLoadAPI<glVertexAttrib4usv>(contextPtr->VertexAttrib4usv);
            contextPtr->VertexAttribPointer = load("glVertexAttribPointer"); context.VertexAttribPointer = internalLoadAPI<glVertexAttribPointer>(contextPtr->VertexAttribPointer);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 2.0 APIs");

        }

        internal static unsafe void LoadGLcore21(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core21)
            {
                InternalIO.InternalAppendLog("The core version 2.1 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->UniformMatrix2x3fv = load("glUniformMatrix2x3fv"); context.UniformMatrix2x3fv = internalLoadAPI<glUniformMatrix2x3fv>(contextPtr->UniformMatrix2x3fv);
            contextPtr->UniformMatrix2x4fv = load("glUniformMatrix2x4fv"); context.UniformMatrix2x4fv = internalLoadAPI<glUniformMatrix2x4fv>(contextPtr->UniformMatrix2x4fv);
            contextPtr->UniformMatrix3x2fv = load("glUniformMatrix3x2fv"); context.UniformMatrix3x2fv = internalLoadAPI<glUniformMatrix3x2fv>(contextPtr->UniformMatrix3x2fv);
            contextPtr->UniformMatrix3x4fv = load("glUniformMatrix3x4fv"); context.UniformMatrix3x4fv = internalLoadAPI<glUniformMatrix3x4fv>(contextPtr->UniformMatrix3x4fv);
            contextPtr->UniformMatrix4x2fv = load("glUniformMatrix4x2fv"); context.UniformMatrix4x2fv = internalLoadAPI<glUniformMatrix4x2fv>(contextPtr->UniformMatrix4x2fv);
            contextPtr->UniformMatrix4x3fv = load("glUniformMatrix4x3fv"); context.UniformMatrix4x3fv = internalLoadAPI<glUniformMatrix4x3fv>(contextPtr->UniformMatrix4x3fv);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 2.1 APIs");

        }

        internal static unsafe void LoadGLcore30(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core30)
            {
                InternalIO.InternalAppendLog("The core version 3.0 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BeginConditionalRender = load("glBeginConditionalRender"); context.BeginConditionalRender = internalLoadAPI<glBeginConditionalRender>(contextPtr->BeginConditionalRender);
            contextPtr->BeginTransformFeedback = load("glBeginTransformFeedback"); context.BeginTransformFeedback = internalLoadAPI<glBeginTransformFeedback>(contextPtr->BeginTransformFeedback);
            contextPtr->BindBufferBase = load("glBindBufferBase"); context.BindBufferBase = internalLoadAPI<glBindBufferBase>(contextPtr->BindBufferBase);
            contextPtr->BindBufferRange = load("glBindBufferRange"); context.BindBufferRange = internalLoadAPI<glBindBufferRange>(contextPtr->BindBufferRange);
            contextPtr->BindFragDataLocation = load("glBindFragDataLocation"); context.BindFragDataLocation = internalLoadAPI<glBindFragDataLocation>(contextPtr->BindFragDataLocation);
            contextPtr->BindFramebuffer = load("glBindFramebuffer"); context.BindFramebuffer = internalLoadAPI<glBindFramebuffer>(contextPtr->BindFramebuffer);
            contextPtr->BindRenderbuffer = load("glBindRenderbuffer"); context.BindRenderbuffer = internalLoadAPI<glBindRenderbuffer>(contextPtr->BindRenderbuffer);
            contextPtr->BindVertexArray = load("glBindVertexArray"); context.BindVertexArray = internalLoadAPI<glBindVertexArray>(contextPtr->BindVertexArray);
            contextPtr->BlitFramebuffer = load("glBlitFramebuffer"); context.BlitFramebuffer = internalLoadAPI<glBlitFramebuffer>(contextPtr->BlitFramebuffer);
            contextPtr->CheckFramebufferStatus = load("glCheckFramebufferStatus"); context.CheckFramebufferStatus = internalLoadAPI<glCheckFramebufferStatus>(contextPtr->CheckFramebufferStatus);
            contextPtr->ClampColor = load("glClampColor"); context.ClampColor = internalLoadAPI<glClampColor>(contextPtr->ClampColor);
            contextPtr->ClearBufferfi = load("glClearBufferfi"); context.ClearBufferfi = internalLoadAPI<glClearBufferfi>(contextPtr->ClearBufferfi);
            contextPtr->ClearBufferfv = load("glClearBufferfv"); context.ClearBufferfv = internalLoadAPI<glClearBufferfv>(contextPtr->ClearBufferfv);
            contextPtr->ClearBufferiv = load("glClearBufferiv"); context.ClearBufferiv = internalLoadAPI<glClearBufferiv>(contextPtr->ClearBufferiv);
            contextPtr->ClearBufferuiv = load("glClearBufferuiv"); context.ClearBufferuiv = internalLoadAPI<glClearBufferuiv>(contextPtr->ClearBufferuiv);
            contextPtr->ColorMaski = load("glColorMaski"); context.ColorMaski = internalLoadAPI<glColorMaski>(contextPtr->ColorMaski);
            contextPtr->DeleteFramebuffers = load("glDeleteFramebuffers"); context.DeleteFramebuffers = internalLoadAPI<glDeleteFramebuffers>(contextPtr->DeleteFramebuffers);
            contextPtr->DeleteRenderbuffers = load("glDeleteRenderbuffers"); context.DeleteRenderbuffers = internalLoadAPI<glDeleteRenderbuffers>(contextPtr->DeleteRenderbuffers);
            contextPtr->DeleteVertexArrays = load("glDeleteVertexArrays"); context.DeleteVertexArrays = internalLoadAPI<glDeleteVertexArrays>(contextPtr->DeleteVertexArrays);
            contextPtr->Disablei = load("glDisablei"); context.Disablei = internalLoadAPI<glDisablei>(contextPtr->Disablei);
            contextPtr->Enablei = load("glEnablei"); context.Enablei = internalLoadAPI<glEnablei>(contextPtr->Enablei);
            contextPtr->EndConditionalRender = load("glEndConditionalRender"); context.EndConditionalRender = internalLoadAPI<glEndConditionalRender>(contextPtr->EndConditionalRender);
            contextPtr->EndTransformFeedback = load("glEndTransformFeedback"); context.EndTransformFeedback = internalLoadAPI<glEndTransformFeedback>(contextPtr->EndTransformFeedback);
            contextPtr->FlushMappedBufferRange = load("glFlushMappedBufferRange"); context.FlushMappedBufferRange = internalLoadAPI<glFlushMappedBufferRange>(contextPtr->FlushMappedBufferRange);
            contextPtr->FramebufferRenderbuffer = load("glFramebufferRenderbuffer"); context.FramebufferRenderbuffer = internalLoadAPI<glFramebufferRenderbuffer>(contextPtr->FramebufferRenderbuffer);
            contextPtr->FramebufferTexture1D = load("glFramebufferTexture1D"); context.FramebufferTexture1D = internalLoadAPI<glFramebufferTexture1D>(contextPtr->FramebufferTexture1D);
            contextPtr->FramebufferTexture2D = load("glFramebufferTexture2D"); context.FramebufferTexture2D = internalLoadAPI<glFramebufferTexture2D>(contextPtr->FramebufferTexture2D);
            contextPtr->FramebufferTexture3D = load("glFramebufferTexture3D"); context.FramebufferTexture3D = internalLoadAPI<glFramebufferTexture3D>(contextPtr->FramebufferTexture3D);
            contextPtr->FramebufferTextureLayer = load("glFramebufferTextureLayer"); context.FramebufferTextureLayer = internalLoadAPI<glFramebufferTextureLayer>(contextPtr->FramebufferTextureLayer);
            contextPtr->GenFramebuffers = load("glGenFramebuffers"); context.GenFramebuffers = internalLoadAPI<glGenFramebuffers>(contextPtr->GenFramebuffers);
            contextPtr->GenRenderbuffers = load("glGenRenderbuffers"); context.GenRenderbuffers = internalLoadAPI<glGenRenderbuffers>(contextPtr->GenRenderbuffers);
            contextPtr->GenVertexArrays = load("glGenVertexArrays"); context.GenVertexArrays = internalLoadAPI<glGenVertexArrays>(contextPtr->GenVertexArrays);
            contextPtr->GenerateMipmap = load("glGenerateMipmap"); context.GenerateMipmap = internalLoadAPI<glGenerateMipmap>(contextPtr->GenerateMipmap);
            contextPtr->GetBooleani_v = load("glGetBooleani_v"); context.GetBooleani_v = internalLoadAPI<glGetBooleani_v>(contextPtr->GetBooleani_v);
            contextPtr->GetFragDataLocation = load("glGetFragDataLocation"); context.GetFragDataLocation = internalLoadAPI<glGetFragDataLocation>(contextPtr->GetFragDataLocation);
            contextPtr->GetFramebufferAttachmentParameteriv = load("glGetFramebufferAttachmentParameteriv"); context.GetFramebufferAttachmentParameteriv = internalLoadAPI<glGetFramebufferAttachmentParameteriv>(contextPtr->GetFramebufferAttachmentParameteriv);
            contextPtr->GetIntegeri_v = load("glGetIntegeri_v"); context.GetIntegeri_v = internalLoadAPI<glGetIntegeri_v>(contextPtr->GetIntegeri_v);
            contextPtr->GetRenderbufferParameteriv = load("glGetRenderbufferParameteriv"); context.GetRenderbufferParameteriv = internalLoadAPI<glGetRenderbufferParameteriv>(contextPtr->GetRenderbufferParameteriv);
            contextPtr->GetStringi = load("glGetStringi"); context.GetStringi = internalLoadAPI<glGetStringi>(contextPtr->GetStringi);
            contextPtr->GetTexParameterIiv = load("glGetTexParameterIiv"); context.GetTexParameterIiv = internalLoadAPI<glGetTexParameterIiv>(contextPtr->GetTexParameterIiv);
            contextPtr->GetTexParameterIuiv = load("glGetTexParameterIuiv"); context.GetTexParameterIuiv = internalLoadAPI<glGetTexParameterIuiv>(contextPtr->GetTexParameterIuiv);
            contextPtr->GetTransformFeedbackVarying = load("glGetTransformFeedbackVarying"); context.GetTransformFeedbackVarying = internalLoadAPI<glGetTransformFeedbackVarying>(contextPtr->GetTransformFeedbackVarying);
            contextPtr->GetUniformuiv = load("glGetUniformuiv"); context.GetUniformuiv = internalLoadAPI<glGetUniformuiv>(contextPtr->GetUniformuiv);
            contextPtr->GetVertexAttribIiv = load("glGetVertexAttribIiv"); context.GetVertexAttribIiv = internalLoadAPI<glGetVertexAttribIiv>(contextPtr->GetVertexAttribIiv);
            contextPtr->GetVertexAttribIuiv = load("glGetVertexAttribIuiv"); context.GetVertexAttribIuiv = internalLoadAPI<glGetVertexAttribIuiv>(contextPtr->GetVertexAttribIuiv);
            contextPtr->IsEnabledi = load("glIsEnabledi"); context.IsEnabledi = internalLoadAPI<glIsEnabledi>(contextPtr->IsEnabledi);
            contextPtr->IsFramebuffer = load("glIsFramebuffer"); context.IsFramebuffer = internalLoadAPI<glIsFramebuffer>(contextPtr->IsFramebuffer);
            contextPtr->IsRenderbuffer = load("glIsRenderbuffer"); context.IsRenderbuffer = internalLoadAPI<glIsRenderbuffer>(contextPtr->IsRenderbuffer);
            contextPtr->IsVertexArray = load("glIsVertexArray"); context.IsVertexArray = internalLoadAPI<glIsVertexArray>(contextPtr->IsVertexArray);
            contextPtr->MapBufferRange = load("glMapBufferRange"); context.MapBufferRange = internalLoadAPI<glMapBufferRange>(contextPtr->MapBufferRange);
            contextPtr->RenderbufferStorage = load("glRenderbufferStorage"); context.RenderbufferStorage = internalLoadAPI<glRenderbufferStorage>(contextPtr->RenderbufferStorage);
            contextPtr->RenderbufferStorageMultisample = load("glRenderbufferStorageMultisample"); context.RenderbufferStorageMultisample = internalLoadAPI<glRenderbufferStorageMultisample>(contextPtr->RenderbufferStorageMultisample);
            contextPtr->TexParameterIiv = load("glTexParameterIiv"); context.TexParameterIiv = internalLoadAPI<glTexParameterIiv>(contextPtr->TexParameterIiv);
            contextPtr->TexParameterIuiv = load("glTexParameterIuiv"); context.TexParameterIuiv = internalLoadAPI<glTexParameterIuiv>(contextPtr->TexParameterIuiv);
            contextPtr->TransformFeedbackVaryings = load("glTransformFeedbackVaryings"); context.TransformFeedbackVaryings = internalLoadAPI<glTransformFeedbackVaryings>(contextPtr->TransformFeedbackVaryings);
            contextPtr->Uniform1ui = load("glUniform1ui"); context.Uniform1ui = internalLoadAPI<glUniform1ui>(contextPtr->Uniform1ui);
            contextPtr->Uniform1uiv = load("glUniform1uiv"); context.Uniform1uiv = internalLoadAPI<glUniform1uiv>(contextPtr->Uniform1uiv);
            contextPtr->Uniform2ui = load("glUniform2ui"); context.Uniform2ui = internalLoadAPI<glUniform2ui>(contextPtr->Uniform2ui);
            contextPtr->Uniform2uiv = load("glUniform2uiv"); context.Uniform2uiv = internalLoadAPI<glUniform2uiv>(contextPtr->Uniform2uiv);
            contextPtr->Uniform3ui = load("glUniform3ui"); context.Uniform3ui = internalLoadAPI<glUniform3ui>(contextPtr->Uniform3ui);
            contextPtr->Uniform3uiv = load("glUniform3uiv"); context.Uniform3uiv = internalLoadAPI<glUniform3uiv>(contextPtr->Uniform3uiv);
            contextPtr->Uniform4ui = load("glUniform4ui"); context.Uniform4ui = internalLoadAPI<glUniform4ui>(contextPtr->Uniform4ui);
            contextPtr->Uniform4uiv = load("glUniform4uiv"); context.Uniform4uiv = internalLoadAPI<glUniform4uiv>(contextPtr->Uniform4uiv);
            contextPtr->VertexAttribI1i = load("glVertexAttribI1i"); context.VertexAttribI1i = internalLoadAPI<glVertexAttribI1i>(contextPtr->VertexAttribI1i);
            contextPtr->VertexAttribI1iv = load("glVertexAttribI1iv"); context.VertexAttribI1iv = internalLoadAPI<glVertexAttribI1iv>(contextPtr->VertexAttribI1iv);
            contextPtr->VertexAttribI1ui = load("glVertexAttribI1ui"); context.VertexAttribI1ui = internalLoadAPI<glVertexAttribI1ui>(contextPtr->VertexAttribI1ui);
            contextPtr->VertexAttribI1uiv = load("glVertexAttribI1uiv"); context.VertexAttribI1uiv = internalLoadAPI<glVertexAttribI1uiv>(contextPtr->VertexAttribI1uiv);
            contextPtr->VertexAttribI2i = load("glVertexAttribI2i"); context.VertexAttribI2i = internalLoadAPI<glVertexAttribI2i>(contextPtr->VertexAttribI2i);
            contextPtr->VertexAttribI2iv = load("glVertexAttribI2iv"); context.VertexAttribI2iv = internalLoadAPI<glVertexAttribI2iv>(contextPtr->VertexAttribI2iv);
            contextPtr->VertexAttribI2ui = load("glVertexAttribI2ui"); context.VertexAttribI2ui = internalLoadAPI<glVertexAttribI2ui>(contextPtr->VertexAttribI2ui);
            contextPtr->VertexAttribI2uiv = load("glVertexAttribI2uiv"); context.VertexAttribI2uiv = internalLoadAPI<glVertexAttribI2uiv>(contextPtr->VertexAttribI2uiv);
            contextPtr->VertexAttribI3i = load("glVertexAttribI3i"); context.VertexAttribI3i = internalLoadAPI<glVertexAttribI3i>(contextPtr->VertexAttribI3i);
            contextPtr->VertexAttribI3iv = load("glVertexAttribI3iv"); context.VertexAttribI3iv = internalLoadAPI<glVertexAttribI3iv>(contextPtr->VertexAttribI3iv);
            contextPtr->VertexAttribI3ui = load("glVertexAttribI3ui"); context.VertexAttribI3ui = internalLoadAPI<glVertexAttribI3ui>(contextPtr->VertexAttribI3ui);
            contextPtr->VertexAttribI3uiv = load("glVertexAttribI3uiv"); context.VertexAttribI3uiv = internalLoadAPI<glVertexAttribI3uiv>(contextPtr->VertexAttribI3uiv);
            contextPtr->VertexAttribI4bv = load("glVertexAttribI4bv"); context.VertexAttribI4bv = internalLoadAPI<glVertexAttribI4bv>(contextPtr->VertexAttribI4bv);
            contextPtr->VertexAttribI4i = load("glVertexAttribI4i"); context.VertexAttribI4i = internalLoadAPI<glVertexAttribI4i>(contextPtr->VertexAttribI4i);
            contextPtr->VertexAttribI4iv = load("glVertexAttribI4iv"); context.VertexAttribI4iv = internalLoadAPI<glVertexAttribI4iv>(contextPtr->VertexAttribI4iv);
            contextPtr->VertexAttribI4sv = load("glVertexAttribI4sv"); context.VertexAttribI4sv = internalLoadAPI<glVertexAttribI4sv>(contextPtr->VertexAttribI4sv);
            contextPtr->VertexAttribI4ubv = load("glVertexAttribI4ubv"); context.VertexAttribI4ubv = internalLoadAPI<glVertexAttribI4ubv>(contextPtr->VertexAttribI4ubv);
            contextPtr->VertexAttribI4ui = load("glVertexAttribI4ui"); context.VertexAttribI4ui = internalLoadAPI<glVertexAttribI4ui>(contextPtr->VertexAttribI4ui);
            contextPtr->VertexAttribI4uiv = load("glVertexAttribI4uiv"); context.VertexAttribI4uiv = internalLoadAPI<glVertexAttribI4uiv>(contextPtr->VertexAttribI4uiv);
            contextPtr->VertexAttribI4usv = load("glVertexAttribI4usv"); context.VertexAttribI4usv = internalLoadAPI<glVertexAttribI4usv>(contextPtr->VertexAttribI4usv);
            contextPtr->VertexAttribIPointer = load("glVertexAttribIPointer"); context.VertexAttribIPointer = internalLoadAPI<glVertexAttribIPointer>(contextPtr->VertexAttribIPointer);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.0 APIs");

        }

        internal static unsafe void LoadGLcore31(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core31)
            {
                InternalIO.InternalAppendLog("The core version 3.1 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindBufferBase = load("glBindBufferBase"); context.BindBufferBase = internalLoadAPI<glBindBufferBase>(contextPtr->BindBufferBase);
            contextPtr->BindBufferRange = load("glBindBufferRange"); context.BindBufferRange = internalLoadAPI<glBindBufferRange>(contextPtr->BindBufferRange);
            contextPtr->CopyBufferSubData = load("glCopyBufferSubData"); context.CopyBufferSubData = internalLoadAPI<glCopyBufferSubData>(contextPtr->CopyBufferSubData);
            contextPtr->DrawArraysInstanced = load("glDrawArraysInstanced"); context.DrawArraysInstanced = internalLoadAPI<glDrawArraysInstanced>(contextPtr->DrawArraysInstanced);
            contextPtr->DrawElementsInstanced = load("glDrawElementsInstanced"); context.DrawElementsInstanced = internalLoadAPI<glDrawElementsInstanced>(contextPtr->DrawElementsInstanced);
            contextPtr->GetActiveUniformBlockName = load("glGetActiveUniformBlockName"); context.GetActiveUniformBlockName = internalLoadAPI<glGetActiveUniformBlockName>(contextPtr->GetActiveUniformBlockName);
            contextPtr->GetActiveUniformBlockiv = load("glGetActiveUniformBlockiv"); context.GetActiveUniformBlockiv = internalLoadAPI<glGetActiveUniformBlockiv>(contextPtr->GetActiveUniformBlockiv);
            contextPtr->GetActiveUniformName = load("glGetActiveUniformName"); context.GetActiveUniformName = internalLoadAPI<glGetActiveUniformName>(contextPtr->GetActiveUniformName);
            contextPtr->GetActiveUniformsiv = load("glGetActiveUniformsiv"); context.GetActiveUniformsiv = internalLoadAPI<glGetActiveUniformsiv>(contextPtr->GetActiveUniformsiv);
            contextPtr->GetIntegeri_v = load("glGetIntegeri_v"); context.GetIntegeri_v = internalLoadAPI<glGetIntegeri_v>(contextPtr->GetIntegeri_v);
            contextPtr->GetUniformBlockIndex = load("glGetUniformBlockIndex"); context.GetUniformBlockIndex = internalLoadAPI<glGetUniformBlockIndex>(contextPtr->GetUniformBlockIndex);
            contextPtr->GetUniformIndices = load("glGetUniformIndices"); context.GetUniformIndices = internalLoadAPI<glGetUniformIndices>(contextPtr->GetUniformIndices);
            contextPtr->PrimitiveRestartIndex = load("glPrimitiveRestartIndex"); context.PrimitiveRestartIndex = internalLoadAPI<glPrimitiveRestartIndex>(contextPtr->PrimitiveRestartIndex);
            contextPtr->TexBuffer = load("glTexBuffer"); context.TexBuffer = internalLoadAPI<glTexBuffer>(contextPtr->TexBuffer);
            contextPtr->UniformBlockBinding = load("glUniformBlockBinding"); context.UniformBlockBinding = internalLoadAPI<glUniformBlockBinding>(contextPtr->UniformBlockBinding);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.1 APIs");
        }

        internal static unsafe void LoadGLcore32(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core32)
            {
                InternalIO.InternalAppendLog("The core version 3.2 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->ClientWaitSync = load("glClientWaitSync"); context.ClientWaitSync = internalLoadAPI<glClientWaitSync>(contextPtr->ClientWaitSync);
            contextPtr->DeleteSync = load("glDeleteSync"); context.DeleteSync = internalLoadAPI<glDeleteSync>(contextPtr->DeleteSync);
            contextPtr->DrawElementsBaseVertex = load("glDrawElementsBaseVertex"); context.DrawElementsBaseVertex = internalLoadAPI<glDrawElementsBaseVertex>(contextPtr->DrawElementsBaseVertex);
            contextPtr->DrawElementsInstancedBaseVertex = load("glDrawElementsInstancedBaseVertex"); context.DrawElementsInstancedBaseVertex = internalLoadAPI<glDrawElementsInstancedBaseVertex>(contextPtr->DrawElementsInstancedBaseVertex);
            contextPtr->DrawRangeElementsBaseVertex = load("glDrawRangeElementsBaseVertex"); context.DrawRangeElementsBaseVertex = internalLoadAPI<glDrawRangeElementsBaseVertex>(contextPtr->DrawRangeElementsBaseVertex);
            contextPtr->FenceSync = load("glFenceSync"); context.FenceSync = internalLoadAPI<glFenceSync>(contextPtr->FenceSync);
            contextPtr->FramebufferTexture = load("glFramebufferTexture"); context.FramebufferTexture = internalLoadAPI<glFramebufferTexture>(contextPtr->FramebufferTexture);
            contextPtr->GetBufferParameteri64v = load("glGetBufferParameteri64v"); context.GetBufferParameteri64v = internalLoadAPI<glGetBufferParameteri64v>(contextPtr->GetBufferParameteri64v);
            contextPtr->GetInteger64i_v = load("glGetInteger64i_v"); context.GetInteger64i_v = internalLoadAPI<glGetInteger64i_v>(contextPtr->GetInteger64i_v);
            contextPtr->GetInteger64v = load("glGetInteger64v"); context.GetInteger64v = internalLoadAPI<glGetInteger64v>(contextPtr->GetInteger64v);
            contextPtr->GetMultisamplefv = load("glGetMultisamplefv"); context.GetMultisamplefv = internalLoadAPI<glGetMultisamplefv>(contextPtr->GetMultisamplefv);
            contextPtr->GetSynciv = load("glGetSynciv"); context.GetSynciv = internalLoadAPI<glGetSynciv>(contextPtr->GetSynciv);
            contextPtr->IsSync = load("glIsSync"); context.IsSync = internalLoadAPI<glIsSync>(contextPtr->IsSync);
            contextPtr->MultiDrawElementsBaseVertex = load("glMultiDrawElementsBaseVertex"); context.MultiDrawElementsBaseVertex = internalLoadAPI<glMultiDrawElementsBaseVertex>(contextPtr->MultiDrawElementsBaseVertex);
            contextPtr->ProvokingVertex = load("glProvokingVertex"); context.ProvokingVertex = internalLoadAPI<glProvokingVertex>(contextPtr->ProvokingVertex);
            contextPtr->SampleMaski = load("glSampleMaski"); context.SampleMaski = internalLoadAPI<glSampleMaski>(contextPtr->SampleMaski);
            contextPtr->TexImage2DMultisample = load("glTexImage2DMultisample"); context.TexImage2DMultisample = internalLoadAPI<glTexImage2DMultisample>(contextPtr->TexImage2DMultisample);
            contextPtr->TexImage3DMultisample = load("glTexImage3DMultisample"); context.TexImage3DMultisample = internalLoadAPI<glTexImage3DMultisample>(contextPtr->TexImage3DMultisample);
            contextPtr->WaitSync = load("glWaitSync"); context.WaitSync = internalLoadAPI<glWaitSync>(contextPtr->WaitSync);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.2 APIs");

        }

        internal static unsafe void LoadGLcore33(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core33)
            {
                InternalIO.InternalAppendLog("The core version 3.3 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindFragDataLocationIndexed = load("glBindFragDataLocationIndexed"); context.BindFragDataLocationIndexed = internalLoadAPI<glBindFragDataLocationIndexed>(contextPtr->BindFragDataLocationIndexed);
            contextPtr->BindSampler = load("glBindSampler"); context.BindSampler = internalLoadAPI<glBindSampler>(contextPtr->BindSampler);
            contextPtr->DeleteSamplers = load("glDeleteSamplers"); context.DeleteSamplers = internalLoadAPI<glDeleteSamplers>(contextPtr->DeleteSamplers);
            contextPtr->GenSamplers = load("glGenSamplers"); context.GenSamplers = internalLoadAPI<glGenSamplers>(contextPtr->GenSamplers);
            contextPtr->GetFragDataIndex = load("glGetFragDataIndex"); context.GetFragDataIndex = internalLoadAPI<glGetFragDataIndex>(contextPtr->GetFragDataIndex);
            contextPtr->GetQueryObjecti64v = load("glGetQueryObjecti64v"); context.GetQueryObjecti64v = internalLoadAPI<glGetQueryObjecti64v>(contextPtr->GetQueryObjecti64v);
            contextPtr->GetQueryObjectui64v = load("glGetQueryObjectui64v"); context.GetQueryObjectui64v = internalLoadAPI<glGetQueryObjectui64v>(contextPtr->GetQueryObjectui64v);
            contextPtr->GetSamplerParameterIiv = load("glGetSamplerParameterIiv"); context.GetSamplerParameterIiv = internalLoadAPI<glGetSamplerParameterIiv>(contextPtr->GetSamplerParameterIiv);
            contextPtr->GetSamplerParameterIuiv = load("glGetSamplerParameterIuiv"); context.GetSamplerParameterIuiv = internalLoadAPI<glGetSamplerParameterIuiv>(contextPtr->GetSamplerParameterIuiv);
            contextPtr->GetSamplerParameterfv = load("glGetSamplerParameterfv"); context.GetSamplerParameterfv = internalLoadAPI<glGetSamplerParameterfv>(contextPtr->GetSamplerParameterfv);
            contextPtr->GetSamplerParameteriv = load("glGetSamplerParameteriv"); context.GetSamplerParameteriv = internalLoadAPI<glGetSamplerParameteriv>(contextPtr->GetSamplerParameteriv);
            contextPtr->IsSampler = load("glIsSampler"); context.IsSampler = internalLoadAPI<glIsSampler>(contextPtr->IsSampler);
            contextPtr->QueryCounter = load("glQueryCounter"); context.QueryCounter = internalLoadAPI<glQueryCounter>(contextPtr->QueryCounter);
            contextPtr->SamplerParameterIiv = load("glSamplerParameterIiv"); context.SamplerParameterIiv = internalLoadAPI<glSamplerParameterIiv>(contextPtr->SamplerParameterIiv);
            contextPtr->SamplerParameterIuiv = load("glSamplerParameterIuiv"); context.SamplerParameterIuiv = internalLoadAPI<glSamplerParameterIuiv>(contextPtr->SamplerParameterIuiv);
            contextPtr->SamplerParameterf = load("glSamplerParameterf"); context.SamplerParameterf = internalLoadAPI<glSamplerParameterf>(contextPtr->SamplerParameterf);
            contextPtr->SamplerParameterfv = load("glSamplerParameterfv"); context.SamplerParameterfv = internalLoadAPI<glSamplerParameterfv>(contextPtr->SamplerParameterfv);
            contextPtr->SamplerParameteri = load("glSamplerParameteri"); context.SamplerParameteri = internalLoadAPI<glSamplerParameteri>(contextPtr->SamplerParameteri);
            contextPtr->SamplerParameteriv = load("glSamplerParameteriv"); context.SamplerParameteriv = internalLoadAPI<glSamplerParameteriv>(contextPtr->SamplerParameteriv);
            contextPtr->VertexAttribDivisor = load("glVertexAttribDivisor"); context.VertexAttribDivisor = internalLoadAPI<glVertexAttribDivisor>(contextPtr->VertexAttribDivisor);
            contextPtr->VertexAttribP1ui = load("glVertexAttribP1ui"); context.VertexAttribP1ui = internalLoadAPI<glVertexAttribP1ui>(contextPtr->VertexAttribP1ui);
            contextPtr->VertexAttribP1uiv = load("glVertexAttribP1uiv"); context.VertexAttribP1uiv = internalLoadAPI<glVertexAttribP1uiv>(contextPtr->VertexAttribP1uiv);
            contextPtr->VertexAttribP2ui = load("glVertexAttribP2ui"); context.VertexAttribP2ui = internalLoadAPI<glVertexAttribP2ui>(contextPtr->VertexAttribP2ui);
            contextPtr->VertexAttribP2uiv = load("glVertexAttribP2uiv"); context.VertexAttribP2uiv = internalLoadAPI<glVertexAttribP2uiv>(contextPtr->VertexAttribP2uiv);
            contextPtr->VertexAttribP3ui = load("glVertexAttribP3ui"); context.VertexAttribP3ui = internalLoadAPI<glVertexAttribP3ui>(contextPtr->VertexAttribP3ui);
            contextPtr->VertexAttribP3uiv = load("glVertexAttribP3uiv"); context.VertexAttribP3uiv = internalLoadAPI<glVertexAttribP3uiv>(contextPtr->VertexAttribP3uiv);
            contextPtr->VertexAttribP4ui = load("glVertexAttribP4ui"); context.VertexAttribP4ui = internalLoadAPI<glVertexAttribP4ui>(contextPtr->VertexAttribP4ui);
            contextPtr->VertexAttribP4uiv = load("glVertexAttribP4uiv"); context.VertexAttribP4uiv = internalLoadAPI<glVertexAttribP4uiv>(contextPtr->VertexAttribP4uiv);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.3 APIs");

        }

        internal static unsafe void LoadGLcore40(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core40)
            {
                InternalIO.InternalAppendLog("The core version 4.0 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BeginQueryIndexed = load("glBeginQueryIndexed"); context.BeginQueryIndexed = internalLoadAPI<glBeginQueryIndexed>(contextPtr->BeginQueryIndexed);
            contextPtr->BindTransformFeedback = load("glBindTransformFeedback"); context.BindTransformFeedback = internalLoadAPI<glBindTransformFeedback>(contextPtr->BindTransformFeedback);
            contextPtr->BlendEquationSeparatei = load("glBlendEquationSeparatei"); context.BlendEquationSeparatei = internalLoadAPI<glBlendEquationSeparatei>(contextPtr->BlendEquationSeparatei);
            contextPtr->BlendEquationi = load("glBlendEquationi"); context.BlendEquationi = internalLoadAPI<glBlendEquationi>(contextPtr->BlendEquationi);
            contextPtr->BlendFuncSeparatei = load("glBlendFuncSeparatei"); context.BlendFuncSeparatei = internalLoadAPI<glBlendFuncSeparatei>(contextPtr->BlendFuncSeparatei);
            contextPtr->BlendFunci = load("glBlendFunci"); context.BlendFunci = internalLoadAPI<glBlendFunci>(contextPtr->BlendFunci);
            contextPtr->DeleteTransformFeedbacks = load("glDeleteTransformFeedbacks"); context.DeleteTransformFeedbacks = internalLoadAPI<glDeleteTransformFeedbacks>(contextPtr->DeleteTransformFeedbacks);
            contextPtr->DrawArraysIndirect = load("glDrawArraysIndirect"); context.DrawArraysIndirect = internalLoadAPI<glDrawArraysIndirect>(contextPtr->DrawArraysIndirect);
            contextPtr->DrawElementsIndirect = load("glDrawElementsIndirect"); context.DrawElementsIndirect = internalLoadAPI<glDrawElementsIndirect>(contextPtr->DrawElementsIndirect);
            contextPtr->DrawTransformFeedback = load("glDrawTransformFeedback"); context.DrawTransformFeedback = internalLoadAPI<glDrawTransformFeedback>(contextPtr->DrawTransformFeedback);
            contextPtr->DrawTransformFeedbackStream = load("glDrawTransformFeedbackStream"); context.DrawTransformFeedbackStream = internalLoadAPI<glDrawTransformFeedbackStream>(contextPtr->DrawTransformFeedbackStream);
            contextPtr->EndQueryIndexed = load("glEndQueryIndexed"); context.EndQueryIndexed = internalLoadAPI<glEndQueryIndexed>(contextPtr->EndQueryIndexed);
            contextPtr->GenTransformFeedbacks = load("glGenTransformFeedbacks"); context.GenTransformFeedbacks = internalLoadAPI<glGenTransformFeedbacks>(contextPtr->GenTransformFeedbacks);
            contextPtr->GetActiveSubroutineName = load("glGetActiveSubroutineName"); context.GetActiveSubroutineName = internalLoadAPI<glGetActiveSubroutineName>(contextPtr->GetActiveSubroutineName);
            contextPtr->GetActiveSubroutineUniformName = load("glGetActiveSubroutineUniformName"); context.GetActiveSubroutineUniformName = internalLoadAPI<glGetActiveSubroutineUniformName>(contextPtr->GetActiveSubroutineUniformName);
            contextPtr->GetActiveSubroutineUniformiv = load("glGetActiveSubroutineUniformiv"); context.GetActiveSubroutineUniformiv = internalLoadAPI<glGetActiveSubroutineUniformiv>(contextPtr->GetActiveSubroutineUniformiv);
            contextPtr->GetProgramStageiv = load("glGetProgramStageiv"); context.GetProgramStageiv = internalLoadAPI<glGetProgramStageiv>(contextPtr->GetProgramStageiv);
            contextPtr->GetQueryIndexediv = load("glGetQueryIndexediv"); context.GetQueryIndexediv = internalLoadAPI<glGetQueryIndexediv>(contextPtr->GetQueryIndexediv);
            contextPtr->GetSubroutineIndex = load("glGetSubroutineIndex"); context.GetSubroutineIndex = internalLoadAPI<glGetSubroutineIndex>(contextPtr->GetSubroutineIndex);
            contextPtr->GetSubroutineUniformLocation = load("glGetSubroutineUniformLocation"); context.GetSubroutineUniformLocation = internalLoadAPI<glGetSubroutineUniformLocation>(contextPtr->GetSubroutineUniformLocation);
            contextPtr->GetUniformSubroutineuiv = load("glGetUniformSubroutineuiv"); context.GetUniformSubroutineuiv = internalLoadAPI<glGetUniformSubroutineuiv>(contextPtr->GetUniformSubroutineuiv);
            contextPtr->GetUniformdv = load("glGetUniformdv"); context.GetUniformdv = internalLoadAPI<glGetUniformdv>(contextPtr->GetUniformdv);
            contextPtr->IsTransformFeedback = load("glIsTransformFeedback"); context.IsTransformFeedback = internalLoadAPI<glIsTransformFeedback>(contextPtr->IsTransformFeedback);
            contextPtr->MinSampleShading = load("glMinSampleShading"); context.MinSampleShading = internalLoadAPI<glMinSampleShading>(contextPtr->MinSampleShading);
            contextPtr->PatchParameterfv = load("glPatchParameterfv"); context.PatchParameterfv = internalLoadAPI<glPatchParameterfv>(contextPtr->PatchParameterfv);
            contextPtr->PatchParameteri = load("glPatchParameteri"); context.PatchParameteri = internalLoadAPI<glPatchParameteri>(contextPtr->PatchParameteri);
            contextPtr->PauseTransformFeedback = load("glPauseTransformFeedback"); context.PauseTransformFeedback = internalLoadAPI<glPauseTransformFeedback>(contextPtr->PauseTransformFeedback);
            contextPtr->ResumeTransformFeedback = load("glResumeTransformFeedback"); context.ResumeTransformFeedback = internalLoadAPI<glResumeTransformFeedback>(contextPtr->ResumeTransformFeedback);
            contextPtr->Uniform1d = load("glUniform1d"); context.Uniform1d = internalLoadAPI<glUniform1d>(contextPtr->Uniform1d);
            contextPtr->Uniform1dv = load("glUniform1dv"); context.Uniform1dv = internalLoadAPI<glUniform1dv>(contextPtr->Uniform1dv);
            contextPtr->Uniform2d = load("glUniform2d"); context.Uniform2d = internalLoadAPI<glUniform2d>(contextPtr->Uniform2d);
            contextPtr->Uniform2dv = load("glUniform2dv"); context.Uniform2dv = internalLoadAPI<glUniform2dv>(contextPtr->Uniform2dv);
            contextPtr->Uniform3d = load("glUniform3d"); context.Uniform3d = internalLoadAPI<glUniform3d>(contextPtr->Uniform3d);
            contextPtr->Uniform3dv = load("glUniform3dv"); context.Uniform3dv = internalLoadAPI<glUniform3dv>(contextPtr->Uniform3dv);
            contextPtr->Uniform4d = load("glUniform4d"); context.Uniform4d = internalLoadAPI<glUniform4d>(contextPtr->Uniform4d);
            contextPtr->Uniform4dv = load("glUniform4dv"); context.Uniform4dv = internalLoadAPI<glUniform4dv>(contextPtr->Uniform4dv);
            contextPtr->UniformMatrix2dv = load("glUniformMatrix2dv"); context.UniformMatrix2dv = internalLoadAPI<glUniformMatrix2dv>(contextPtr->UniformMatrix2dv);
            contextPtr->UniformMatrix2x3dv = load("glUniformMatrix2x3dv"); context.UniformMatrix2x3dv = internalLoadAPI<glUniformMatrix2x3dv>(contextPtr->UniformMatrix2x3dv);
            contextPtr->UniformMatrix2x4dv = load("glUniformMatrix2x4dv"); context.UniformMatrix2x4dv = internalLoadAPI<glUniformMatrix2x4dv>(contextPtr->UniformMatrix2x4dv);
            contextPtr->UniformMatrix3dv = load("glUniformMatrix3dv"); context.UniformMatrix3dv = internalLoadAPI<glUniformMatrix3dv>(contextPtr->UniformMatrix3dv);
            contextPtr->UniformMatrix3x2dv = load("glUniformMatrix3x2dv"); context.UniformMatrix3x2dv = internalLoadAPI<glUniformMatrix3x2dv>(contextPtr->UniformMatrix3x2dv);
            contextPtr->UniformMatrix3x4dv = load("glUniformMatrix3x4dv"); context.UniformMatrix3x4dv = internalLoadAPI<glUniformMatrix3x4dv>(contextPtr->UniformMatrix3x4dv);
            contextPtr->UniformMatrix4dv = load("glUniformMatrix4dv"); context.UniformMatrix4dv = internalLoadAPI<glUniformMatrix4dv>(contextPtr->UniformMatrix4dv);
            contextPtr->UniformMatrix4x2dv = load("glUniformMatrix4x2dv"); context.UniformMatrix4x2dv = internalLoadAPI<glUniformMatrix4x2dv>(contextPtr->UniformMatrix4x2dv);
            contextPtr->UniformMatrix4x3dv = load("glUniformMatrix4x3dv"); context.UniformMatrix4x3dv = internalLoadAPI<glUniformMatrix4x3dv>(contextPtr->UniformMatrix4x3dv);
            contextPtr->UniformSubroutinesuiv = load("glUniformSubroutinesuiv"); context.UniformSubroutinesuiv = internalLoadAPI<glUniformSubroutinesuiv>(contextPtr->UniformSubroutinesuiv);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.0 APIs");
        }

        internal static unsafe void LoadGLcore41(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core41)
            {
                InternalIO.InternalAppendLog("The core version 4.1 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->ActiveShaderProgram = load("glActiveShaderProgram"); context.ActiveShaderProgram = internalLoadAPI<glActiveShaderProgram>(contextPtr->ActiveShaderProgram);
            contextPtr->BindProgramPipeline = load("glBindProgramPipeline"); context.BindProgramPipeline = internalLoadAPI<glBindProgramPipeline>(contextPtr->BindProgramPipeline);
            contextPtr->ClearDepthf = load("glClearDepthf"); context.ClearDepthf = internalLoadAPI<glClearDepthf>(contextPtr->ClearDepthf);
            contextPtr->CreateShaderProgramv = load("glCreateShaderProgramv"); context.CreateShaderProgramv = internalLoadAPI<glCreateShaderProgramv>(contextPtr->CreateShaderProgramv);
            contextPtr->DeleteProgramPipelines = load("glDeleteProgramPipelines"); context.DeleteProgramPipelines = internalLoadAPI<glDeleteProgramPipelines>(contextPtr->DeleteProgramPipelines);
            contextPtr->DepthRangeArrayv = load("glDepthRangeArrayv"); context.DepthRangeArrayv = internalLoadAPI<glDepthRangeArrayv>(contextPtr->DepthRangeArrayv);
            contextPtr->DepthRangeIndexed = load("glDepthRangeIndexed"); context.DepthRangeIndexed = internalLoadAPI<glDepthRangeIndexed>(contextPtr->DepthRangeIndexed);
            contextPtr->DepthRangef = load("glDepthRangef"); context.DepthRangef = internalLoadAPI<glDepthRangef>(contextPtr->DepthRangef);
            contextPtr->GenProgramPipelines = load("glGenProgramPipelines"); context.GenProgramPipelines = internalLoadAPI<glGenProgramPipelines>(contextPtr->GenProgramPipelines);
            contextPtr->GetDoublei_v = load("glGetDoublei_v"); context.GetDoublei_v = internalLoadAPI<glGetDoublei_v>(contextPtr->GetDoublei_v);
            contextPtr->GetFloati_v = load("glGetFloati_v"); context.GetFloati_v = internalLoadAPI<glGetFloati_v>(contextPtr->GetFloati_v);
            contextPtr->GetProgramBinary = load("glGetProgramBinary"); context.GetProgramBinary = internalLoadAPI<glGetProgramBinary>(contextPtr->GetProgramBinary);
            contextPtr->GetProgramPipelineInfoLog = load("glGetProgramPipelineInfoLog"); context.GetProgramPipelineInfoLog = internalLoadAPI<glGetProgramPipelineInfoLog>(contextPtr->GetProgramPipelineInfoLog);
            contextPtr->GetProgramPipelineiv = load("glGetProgramPipelineiv"); context.GetProgramPipelineiv = internalLoadAPI<glGetProgramPipelineiv>(contextPtr->GetProgramPipelineiv);
            contextPtr->GetShaderPrecisionFormat = load("glGetShaderPrecisionFormat"); context.GetShaderPrecisionFormat = internalLoadAPI<glGetShaderPrecisionFormat>(contextPtr->GetShaderPrecisionFormat);
            contextPtr->GetVertexAttribLdv = load("glGetVertexAttribLdv"); context.GetVertexAttribLdv = internalLoadAPI<glGetVertexAttribLdv>(contextPtr->GetVertexAttribLdv);
            contextPtr->IsProgramPipeline = load("glIsProgramPipeline"); context.IsProgramPipeline = internalLoadAPI<glIsProgramPipeline>(contextPtr->IsProgramPipeline);
            contextPtr->ProgramBinary = load("glProgramBinary"); context.ProgramBinary = internalLoadAPI<glProgramBinary>(contextPtr->ProgramBinary);
            contextPtr->ProgramParameteri = load("glProgramParameteri"); context.ProgramParameteri = internalLoadAPI<glProgramParameteri>(contextPtr->ProgramParameteri);
            contextPtr->ProgramUniform1d = load("glProgramUniform1d"); context.ProgramUniform1d = internalLoadAPI<glProgramUniform1d>(contextPtr->ProgramUniform1d);
            contextPtr->ProgramUniform1dv = load("glProgramUniform1dv"); context.ProgramUniform1dv = internalLoadAPI<glProgramUniform1dv>(contextPtr->ProgramUniform1dv);
            contextPtr->ProgramUniform1f = load("glProgramUniform1f"); context.ProgramUniform1f = internalLoadAPI<glProgramUniform1f>(contextPtr->ProgramUniform1f);
            contextPtr->ProgramUniform1fv = load("glProgramUniform1fv"); context.ProgramUniform1fv = internalLoadAPI<glProgramUniform1fv>(contextPtr->ProgramUniform1fv);
            contextPtr->ProgramUniform1i = load("glProgramUniform1i"); context.ProgramUniform1i = internalLoadAPI<glProgramUniform1i>(contextPtr->ProgramUniform1i);
            contextPtr->ProgramUniform1iv = load("glProgramUniform1iv"); context.ProgramUniform1iv = internalLoadAPI<glProgramUniform1iv>(contextPtr->ProgramUniform1iv);
            contextPtr->ProgramUniform1ui = load("glProgramUniform1ui"); context.ProgramUniform1ui = internalLoadAPI<glProgramUniform1ui>(contextPtr->ProgramUniform1ui);
            contextPtr->ProgramUniform1uiv = load("glProgramUniform1uiv"); context.ProgramUniform1uiv = internalLoadAPI<glProgramUniform1uiv>(contextPtr->ProgramUniform1uiv);
            contextPtr->ProgramUniform2d = load("glProgramUniform2d"); context.ProgramUniform2d = internalLoadAPI<glProgramUniform2d>(contextPtr->ProgramUniform2d);
            contextPtr->ProgramUniform2dv = load("glProgramUniform2dv"); context.ProgramUniform2dv = internalLoadAPI<glProgramUniform2dv>(contextPtr->ProgramUniform2dv);
            contextPtr->ProgramUniform2f = load("glProgramUniform2f"); context.ProgramUniform2f = internalLoadAPI<glProgramUniform2f>(contextPtr->ProgramUniform2f);
            contextPtr->ProgramUniform2fv = load("glProgramUniform2fv"); context.ProgramUniform2fv = internalLoadAPI<glProgramUniform2fv>(contextPtr->ProgramUniform2fv);
            contextPtr->ProgramUniform2i = load("glProgramUniform2i"); context.ProgramUniform2i = internalLoadAPI<glProgramUniform2i>(contextPtr->ProgramUniform2i);
            contextPtr->ProgramUniform2iv = load("glProgramUniform2iv"); context.ProgramUniform2iv = internalLoadAPI<glProgramUniform2iv>(contextPtr->ProgramUniform2iv);
            contextPtr->ProgramUniform2ui = load("glProgramUniform2ui"); context.ProgramUniform2ui = internalLoadAPI<glProgramUniform2ui>(contextPtr->ProgramUniform2ui);
            contextPtr->ProgramUniform2uiv = load("glProgramUniform2uiv"); context.ProgramUniform2uiv = internalLoadAPI<glProgramUniform2uiv>(contextPtr->ProgramUniform2uiv);
            contextPtr->ProgramUniform3d = load("glProgramUniform3d"); context.ProgramUniform3d = internalLoadAPI<glProgramUniform3d>(contextPtr->ProgramUniform3d);
            contextPtr->ProgramUniform3dv = load("glProgramUniform3dv"); context.ProgramUniform3dv = internalLoadAPI<glProgramUniform3dv>(contextPtr->ProgramUniform3dv);
            contextPtr->ProgramUniform3f = load("glProgramUniform3f"); context.ProgramUniform3f = internalLoadAPI<glProgramUniform3f>(contextPtr->ProgramUniform3f);
            contextPtr->ProgramUniform3fv = load("glProgramUniform3fv"); context.ProgramUniform3fv = internalLoadAPI<glProgramUniform3fv>(contextPtr->ProgramUniform3fv);
            contextPtr->ProgramUniform3i = load("glProgramUniform3i"); context.ProgramUniform3i = internalLoadAPI<glProgramUniform3i>(contextPtr->ProgramUniform3i);
            contextPtr->ProgramUniform3iv = load("glProgramUniform3iv"); context.ProgramUniform3iv = internalLoadAPI<glProgramUniform3iv>(contextPtr->ProgramUniform3iv);
            contextPtr->ProgramUniform3ui = load("glProgramUniform3ui"); context.ProgramUniform3ui = internalLoadAPI<glProgramUniform3ui>(contextPtr->ProgramUniform3ui);
            contextPtr->ProgramUniform3uiv = load("glProgramUniform3uiv"); context.ProgramUniform3uiv = internalLoadAPI<glProgramUniform3uiv>(contextPtr->ProgramUniform3uiv);
            contextPtr->ProgramUniform4d = load("glProgramUniform4d"); context.ProgramUniform4d = internalLoadAPI<glProgramUniform4d>(contextPtr->ProgramUniform4d);
            contextPtr->ProgramUniform4dv = load("glProgramUniform4dv"); context.ProgramUniform4dv = internalLoadAPI<glProgramUniform4dv>(contextPtr->ProgramUniform4dv);
            contextPtr->ProgramUniform4f = load("glProgramUniform4f"); context.ProgramUniform4f = internalLoadAPI<glProgramUniform4f>(contextPtr->ProgramUniform4f);
            contextPtr->ProgramUniform4fv = load("glProgramUniform4fv"); context.ProgramUniform4fv = internalLoadAPI<glProgramUniform4fv>(contextPtr->ProgramUniform4fv);
            contextPtr->ProgramUniform4i = load("glProgramUniform4i"); context.ProgramUniform4i = internalLoadAPI<glProgramUniform4i>(contextPtr->ProgramUniform4i);
            contextPtr->ProgramUniform4iv = load("glProgramUniform4iv"); context.ProgramUniform4iv = internalLoadAPI<glProgramUniform4iv>(contextPtr->ProgramUniform4iv);
            contextPtr->ProgramUniform4ui = load("glProgramUniform4ui"); context.ProgramUniform4ui = internalLoadAPI<glProgramUniform4ui>(contextPtr->ProgramUniform4ui);
            contextPtr->ProgramUniform4uiv = load("glProgramUniform4uiv"); context.ProgramUniform4uiv = internalLoadAPI<glProgramUniform4uiv>(contextPtr->ProgramUniform4uiv);
            contextPtr->ProgramUniformMatrix2dv = load("glProgramUniformMatrix2dv"); context.ProgramUniformMatrix2dv = internalLoadAPI<glProgramUniformMatrix2dv>(contextPtr->ProgramUniformMatrix2dv);
            contextPtr->ProgramUniformMatrix2fv = load("glProgramUniformMatrix2fv"); context.ProgramUniformMatrix2fv = internalLoadAPI<glProgramUniformMatrix2fv>(contextPtr->ProgramUniformMatrix2fv);
            contextPtr->ProgramUniformMatrix2x3dv = load("glProgramUniformMatrix2x3dv"); context.ProgramUniformMatrix2x3dv = internalLoadAPI<glProgramUniformMatrix2x3dv>(contextPtr->ProgramUniformMatrix2x3dv);
            contextPtr->ProgramUniformMatrix2x3fv = load("glProgramUniformMatrix2x3fv"); context.ProgramUniformMatrix2x3fv = internalLoadAPI<glProgramUniformMatrix2x3fv>(contextPtr->ProgramUniformMatrix2x3fv);
            contextPtr->ProgramUniformMatrix2x4dv = load("glProgramUniformMatrix2x4dv"); context.ProgramUniformMatrix2x4dv = internalLoadAPI<glProgramUniformMatrix2x4dv>(contextPtr->ProgramUniformMatrix2x4dv);
            contextPtr->ProgramUniformMatrix2x4fv = load("glProgramUniformMatrix2x4fv"); context.ProgramUniformMatrix2x4fv = internalLoadAPI<glProgramUniformMatrix2x4fv>(contextPtr->ProgramUniformMatrix2x4fv);
            contextPtr->ProgramUniformMatrix3dv = load("glProgramUniformMatrix3dv"); context.ProgramUniformMatrix3dv = internalLoadAPI<glProgramUniformMatrix3dv>(contextPtr->ProgramUniformMatrix3dv);
            contextPtr->ProgramUniformMatrix3fv = load("glProgramUniformMatrix3fv"); context.ProgramUniformMatrix3fv = internalLoadAPI<glProgramUniformMatrix3fv>(contextPtr->ProgramUniformMatrix3fv);
            contextPtr->ProgramUniformMatrix3x2dv = load("glProgramUniformMatrix3x2dv"); context.ProgramUniformMatrix3x2dv = internalLoadAPI<glProgramUniformMatrix3x2dv>(contextPtr->ProgramUniformMatrix3x2dv);
            contextPtr->ProgramUniformMatrix3x2fv = load("glProgramUniformMatrix3x2fv"); context.ProgramUniformMatrix3x2fv = internalLoadAPI<glProgramUniformMatrix3x2fv>(contextPtr->ProgramUniformMatrix3x2fv);
            contextPtr->ProgramUniformMatrix3x4dv = load("glProgramUniformMatrix3x4dv"); context.ProgramUniformMatrix3x4dv = internalLoadAPI<glProgramUniformMatrix3x4dv>(contextPtr->ProgramUniformMatrix3x4dv);
            contextPtr->ProgramUniformMatrix3x4fv = load("glProgramUniformMatrix3x4fv"); context.ProgramUniformMatrix3x4fv = internalLoadAPI<glProgramUniformMatrix3x4fv>(contextPtr->ProgramUniformMatrix3x4fv);
            contextPtr->ProgramUniformMatrix4dv = load("glProgramUniformMatrix4dv"); context.ProgramUniformMatrix4dv = internalLoadAPI<glProgramUniformMatrix4dv>(contextPtr->ProgramUniformMatrix4dv);
            contextPtr->ProgramUniformMatrix4fv = load("glProgramUniformMatrix4fv"); context.ProgramUniformMatrix4fv = internalLoadAPI<glProgramUniformMatrix4fv>(contextPtr->ProgramUniformMatrix4fv);
            contextPtr->ProgramUniformMatrix4x2dv = load("glProgramUniformMatrix4x2dv"); context.ProgramUniformMatrix4x2dv = internalLoadAPI<glProgramUniformMatrix4x2dv>(contextPtr->ProgramUniformMatrix4x2dv);
            contextPtr->ProgramUniformMatrix4x2fv = load("glProgramUniformMatrix4x2fv"); context.ProgramUniformMatrix4x2fv = internalLoadAPI<glProgramUniformMatrix4x2fv>(contextPtr->ProgramUniformMatrix4x2fv);
            contextPtr->ProgramUniformMatrix4x3dv = load("glProgramUniformMatrix4x3dv"); context.ProgramUniformMatrix4x3dv = internalLoadAPI<glProgramUniformMatrix4x3dv>(contextPtr->ProgramUniformMatrix4x3dv);
            contextPtr->ProgramUniformMatrix4x3fv = load("glProgramUniformMatrix4x3fv"); context.ProgramUniformMatrix4x3fv = internalLoadAPI<glProgramUniformMatrix4x3fv>(contextPtr->ProgramUniformMatrix4x3fv);
            contextPtr->ReleaseShaderCompiler = load("glReleaseShaderCompiler"); context.ReleaseShaderCompiler = internalLoadAPI<glReleaseShaderCompiler>(contextPtr->ReleaseShaderCompiler);
            contextPtr->ScissorArrayv = load("glScissorArrayv"); context.ScissorArrayv = internalLoadAPI<glScissorArrayv>(contextPtr->ScissorArrayv);
            contextPtr->ScissorIndexed = load("glScissorIndexed"); context.ScissorIndexed = internalLoadAPI<glScissorIndexed>(contextPtr->ScissorIndexed);
            contextPtr->ScissorIndexedv = load("glScissorIndexedv"); context.ScissorIndexedv = internalLoadAPI<glScissorIndexedv>(contextPtr->ScissorIndexedv);
            contextPtr->ShaderBinary = load("glShaderBinary"); context.ShaderBinary = internalLoadAPI<glShaderBinary>(contextPtr->ShaderBinary);
            contextPtr->UseProgramStages = load("glUseProgramStages"); context.UseProgramStages = internalLoadAPI<glUseProgramStages>(contextPtr->UseProgramStages);
            contextPtr->ValidateProgramPipeline = load("glValidateProgramPipeline"); context.ValidateProgramPipeline = internalLoadAPI<glValidateProgramPipeline>(contextPtr->ValidateProgramPipeline);
            contextPtr->VertexAttribL1d = load("glVertexAttribL1d"); context.VertexAttribL1d = internalLoadAPI<glVertexAttribL1d>(contextPtr->VertexAttribL1d);
            contextPtr->VertexAttribL1dv = load("glVertexAttribL1dv"); context.VertexAttribL1dv = internalLoadAPI<glVertexAttribL1dv>(contextPtr->VertexAttribL1dv);
            contextPtr->VertexAttribL2d = load("glVertexAttribL2d"); context.VertexAttribL2d = internalLoadAPI<glVertexAttribL2d>(contextPtr->VertexAttribL2d);
            contextPtr->VertexAttribL2dv = load("glVertexAttribL2dv"); context.VertexAttribL2dv = internalLoadAPI<glVertexAttribL2dv>(contextPtr->VertexAttribL2dv);
            contextPtr->VertexAttribL3d = load("glVertexAttribL3d"); context.VertexAttribL3d = internalLoadAPI<glVertexAttribL3d>(contextPtr->VertexAttribL3d);
            contextPtr->VertexAttribL3dv = load("glVertexAttribL3dv"); context.VertexAttribL3dv = internalLoadAPI<glVertexAttribL3dv>(contextPtr->VertexAttribL3dv);
            contextPtr->VertexAttribL4d = load("glVertexAttribL4d"); context.VertexAttribL4d = internalLoadAPI<glVertexAttribL4d>(contextPtr->VertexAttribL4d);
            contextPtr->VertexAttribL4dv = load("glVertexAttribL4dv"); context.VertexAttribL4dv = internalLoadAPI<glVertexAttribL4dv>(contextPtr->VertexAttribL4dv);
            contextPtr->VertexAttribLPointer = load("glVertexAttribLPointer"); context.VertexAttribLPointer = internalLoadAPI<glVertexAttribLPointer>(contextPtr->VertexAttribLPointer);
            contextPtr->ViewportArrayv = load("glViewportArrayv"); context.ViewportArrayv = internalLoadAPI<glViewportArrayv>(contextPtr->ViewportArrayv);
            contextPtr->ViewportIndexedf = load("glViewportIndexedf"); context.ViewportIndexedf = internalLoadAPI<glViewportIndexedf>(contextPtr->ViewportIndexedf);
            contextPtr->ViewportIndexedfv = load("glViewportIndexedfv"); context.ViewportIndexedfv = internalLoadAPI<glViewportIndexedfv>(contextPtr->ViewportIndexedfv);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.1 APIs");
        }

        internal static unsafe void LoadGLcore42(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core42)
            {
                InternalIO.InternalAppendLog("The core version 4.2 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindImageTexture = load("glBindImageTexture"); context.BindImageTexture = internalLoadAPI<glBindImageTexture>(contextPtr->BindImageTexture);
            contextPtr->DrawArraysInstancedBaseInstance = load("glDrawArraysInstancedBaseInstance"); context.DrawArraysInstancedBaseInstance = internalLoadAPI<glDrawArraysInstancedBaseInstance>(contextPtr->DrawArraysInstancedBaseInstance);
            contextPtr->DrawElementsInstancedBaseInstance = load("glDrawElementsInstancedBaseInstance"); context.DrawElementsInstancedBaseInstance = internalLoadAPI<glDrawElementsInstancedBaseInstance>(contextPtr->DrawElementsInstancedBaseInstance);
            contextPtr->DrawElementsInstancedBaseVertexBaseInstance = load("glDrawElementsInstancedBaseVertexBaseInstance"); context.DrawElementsInstancedBaseVertexBaseInstance = internalLoadAPI<glDrawElementsInstancedBaseVertexBaseInstance>(contextPtr->DrawElementsInstancedBaseVertexBaseInstance);
            contextPtr->DrawTransformFeedbackInstanced = load("glDrawTransformFeedbackInstanced"); context.DrawTransformFeedbackInstanced = internalLoadAPI<glDrawTransformFeedbackInstanced>(contextPtr->DrawTransformFeedbackInstanced);
            contextPtr->DrawTransformFeedbackStreamInstanced = load("glDrawTransformFeedbackStreamInstanced"); context.DrawTransformFeedbackStreamInstanced = internalLoadAPI<glDrawTransformFeedbackStreamInstanced>(contextPtr->DrawTransformFeedbackStreamInstanced);
            contextPtr->GetActiveAtomicCounterBufferiv = load("glGetActiveAtomicCounterBufferiv"); context.GetActiveAtomicCounterBufferiv = internalLoadAPI<glGetActiveAtomicCounterBufferiv>(contextPtr->GetActiveAtomicCounterBufferiv);
            contextPtr->GetInternalformativ = load("glGetInternalformativ"); context.GetInternalformativ = internalLoadAPI<glGetInternalformativ>(contextPtr->GetInternalformativ);
            contextPtr->MemoryBarrier = load("glMemoryBarrier"); context.MemoryBarrier = internalLoadAPI<glMemoryBarrier>(contextPtr->MemoryBarrier);
            contextPtr->TexStorage1D = load("glTexStorage1D"); context.TexStorage1D = internalLoadAPI<glTexStorage1D>(contextPtr->TexStorage1D);
            contextPtr->TexStorage2D = load("glTexStorage2D"); context.TexStorage2D = internalLoadAPI<glTexStorage2D>(contextPtr->TexStorage2D);
            contextPtr->TexStorage3D = load("glTexStorage3D"); context.TexStorage3D = internalLoadAPI<glTexStorage3D>(contextPtr->TexStorage3D);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.2 APIs");
        }

        internal static unsafe void LoadGLcore43(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core43)
            {
                InternalIO.InternalAppendLog("The core version 4.3 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindVertexBuffer = load("glBindVertexBuffer"); context.BindVertexBuffer = internalLoadAPI<glBindVertexBuffer>(contextPtr->BindVertexBuffer);
            contextPtr->ClearBufferData = load("glClearBufferData"); context.ClearBufferData = internalLoadAPI<glClearBufferData>(contextPtr->ClearBufferData);
            contextPtr->ClearBufferSubData = load("glClearBufferSubData"); context.ClearBufferSubData = internalLoadAPI<glClearBufferSubData>(contextPtr->ClearBufferSubData);
            contextPtr->CopyImageSubData = load("glCopyImageSubData"); context.CopyImageSubData = internalLoadAPI<glCopyImageSubData>(contextPtr->CopyImageSubData);
            contextPtr->DebugMessageCallback = load("glDebugMessageCallback"); context.DebugMessageCallback = internalLoadAPI<glDebugMessageCallback>(contextPtr->DebugMessageCallback);
            contextPtr->DebugMessageControl = load("glDebugMessageControl"); context.DebugMessageControl = internalLoadAPI<glDebugMessageControl>(contextPtr->DebugMessageControl);
            contextPtr->DebugMessageInsert = load("glDebugMessageInsert"); context.DebugMessageInsert = internalLoadAPI<glDebugMessageInsert>(contextPtr->DebugMessageInsert);
            contextPtr->DispatchCompute = load("glDispatchCompute"); context.DispatchCompute = internalLoadAPI<glDispatchCompute>(contextPtr->DispatchCompute);
            contextPtr->DispatchComputeIndirect = load("glDispatchComputeIndirect"); context.DispatchComputeIndirect = internalLoadAPI<glDispatchComputeIndirect>(contextPtr->DispatchComputeIndirect);
            contextPtr->FramebufferParameteri = load("glFramebufferParameteri"); context.FramebufferParameteri = internalLoadAPI<glFramebufferParameteri>(contextPtr->FramebufferParameteri);
            contextPtr->GetDebugMessageLog = load("glGetDebugMessageLog"); context.GetDebugMessageLog = internalLoadAPI<glGetDebugMessageLog>(contextPtr->GetDebugMessageLog);
            contextPtr->GetFramebufferParameteriv = load("glGetFramebufferParameteriv"); context.GetFramebufferParameteriv = internalLoadAPI<glGetFramebufferParameteriv>(contextPtr->GetFramebufferParameteriv);
            contextPtr->GetInternalformati64v = load("glGetInternalformati64v"); context.GetInternalformati64v = internalLoadAPI<glGetInternalformati64v>(contextPtr->GetInternalformati64v);
            contextPtr->GetObjectLabel = load("glGetObjectLabel"); context.GetObjectLabel = internalLoadAPI<glGetObjectLabel>(contextPtr->GetObjectLabel);
            contextPtr->GetObjectPtrLabel = load("glGetObjectPtrLabel"); context.GetObjectPtrLabel = internalLoadAPI<glGetObjectPtrLabel>(contextPtr->GetObjectPtrLabel);
            contextPtr->GetPointerv = load("glGetPointerv"); context.GetPointerv = internalLoadAPI<glGetPointerv>(contextPtr->GetPointerv);
            contextPtr->GetProgramInterfaceiv = load("glGetProgramInterfaceiv"); context.GetProgramInterfaceiv = internalLoadAPI<glGetProgramInterfaceiv>(contextPtr->GetProgramInterfaceiv);
            contextPtr->GetProgramResourceIndex = load("glGetProgramResourceIndex"); context.GetProgramResourceIndex = internalLoadAPI<glGetProgramResourceIndex>(contextPtr->GetProgramResourceIndex);
            contextPtr->GetProgramResourceLocation = load("glGetProgramResourceLocation"); context.GetProgramResourceLocation = internalLoadAPI<glGetProgramResourceLocation>(contextPtr->GetProgramResourceLocation);
            contextPtr->GetProgramResourceLocationIndex = load("glGetProgramResourceLocationIndex"); context.GetProgramResourceLocationIndex = internalLoadAPI<glGetProgramResourceLocationIndex>(contextPtr->GetProgramResourceLocationIndex);
            contextPtr->GetProgramResourceName = load("glGetProgramResourceName"); context.GetProgramResourceName = internalLoadAPI<glGetProgramResourceName>(contextPtr->GetProgramResourceName);
            contextPtr->GetProgramResourceiv = load("glGetProgramResourceiv"); context.GetProgramResourceiv = internalLoadAPI<glGetProgramResourceiv>(contextPtr->GetProgramResourceiv);
            contextPtr->InvalidateBufferData = load("glInvalidateBufferData"); context.InvalidateBufferData = internalLoadAPI<glInvalidateBufferData>(contextPtr->InvalidateBufferData);
            contextPtr->InvalidateBufferSubData = load("glInvalidateBufferSubData"); context.InvalidateBufferSubData = internalLoadAPI<glInvalidateBufferSubData>(contextPtr->InvalidateBufferSubData);
            contextPtr->InvalidateFramebuffer = load("glInvalidateFramebuffer"); context.InvalidateFramebuffer = internalLoadAPI<glInvalidateFramebuffer>(contextPtr->InvalidateFramebuffer);
            contextPtr->InvalidateSubFramebuffer = load("glInvalidateSubFramebuffer"); context.InvalidateSubFramebuffer = internalLoadAPI<glInvalidateSubFramebuffer>(contextPtr->InvalidateSubFramebuffer);
            contextPtr->InvalidateTexImage = load("glInvalidateTexImage"); context.InvalidateTexImage = internalLoadAPI<glInvalidateTexImage>(contextPtr->InvalidateTexImage);
            contextPtr->InvalidateTexSubImage = load("glInvalidateTexSubImage"); context.InvalidateTexSubImage = internalLoadAPI<glInvalidateTexSubImage>(contextPtr->InvalidateTexSubImage);
            contextPtr->MultiDrawArraysIndirect = load("glMultiDrawArraysIndirect"); context.MultiDrawArraysIndirect = internalLoadAPI<glMultiDrawArraysIndirect>(contextPtr->MultiDrawArraysIndirect);
            contextPtr->MultiDrawElementsIndirect = load("glMultiDrawElementsIndirect"); context.MultiDrawElementsIndirect = internalLoadAPI<glMultiDrawElementsIndirect>(contextPtr->MultiDrawElementsIndirect);
            contextPtr->ObjectLabel = load("glObjectLabel"); context.ObjectLabel = internalLoadAPI<glObjectLabel>(contextPtr->ObjectLabel);
            contextPtr->ObjectPtrLabel = load("glObjectPtrLabel"); context.ObjectPtrLabel = internalLoadAPI<glObjectPtrLabel>(contextPtr->ObjectPtrLabel);
            contextPtr->PopDebugGroup = load("glPopDebugGroup"); context.PopDebugGroup = internalLoadAPI<glPopDebugGroup>(contextPtr->PopDebugGroup);
            contextPtr->PushDebugGroup = load("glPushDebugGroup"); context.PushDebugGroup = internalLoadAPI<glPushDebugGroup>(contextPtr->PushDebugGroup);
            contextPtr->ShaderStorageBlockBinding = load("glShaderStorageBlockBinding"); context.ShaderStorageBlockBinding = internalLoadAPI<glShaderStorageBlockBinding>(contextPtr->ShaderStorageBlockBinding);
            contextPtr->TexBufferRange = load("glTexBufferRange"); context.TexBufferRange = internalLoadAPI<glTexBufferRange>(contextPtr->TexBufferRange);
            contextPtr->TexStorage2DMultisample = load("glTexStorage2DMultisample"); context.TexStorage2DMultisample = internalLoadAPI<glTexStorage2DMultisample>(contextPtr->TexStorage2DMultisample);
            contextPtr->TexStorage3DMultisample = load("glTexStorage3DMultisample"); context.TexStorage3DMultisample = internalLoadAPI<glTexStorage3DMultisample>(contextPtr->TexStorage3DMultisample);
            contextPtr->TextureView = load("glTextureView"); context.TextureView = internalLoadAPI<glTextureView>(contextPtr->TextureView);
            contextPtr->VertexAttribBinding = load("glVertexAttribBinding"); context.VertexAttribBinding = internalLoadAPI<glVertexAttribBinding>(contextPtr->VertexAttribBinding);
            contextPtr->VertexAttribFormat = load("glVertexAttribFormat"); context.VertexAttribFormat = internalLoadAPI<glVertexAttribFormat>(contextPtr->VertexAttribFormat);
            contextPtr->VertexAttribIFormat = load("glVertexAttribIFormat"); context.VertexAttribIFormat = internalLoadAPI<glVertexAttribIFormat>(contextPtr->VertexAttribIFormat);
            contextPtr->ClearBufferData = load("glClearBufferData"); context.ClearBufferData = internalLoadAPI<glClearBufferData>(contextPtr->ClearBufferData);
            contextPtr->VertexBindingDivisor = load("glVertexBindingDivisor"); context.VertexBindingDivisor = internalLoadAPI<glVertexBindingDivisor>(contextPtr->VertexBindingDivisor);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.3 APIs");
        }

        internal static unsafe void LoadGLcore44(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core44)
            {
                InternalIO.InternalAppendLog("The core version 4.4 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindBuffersBase = load("glBindBuffersBase"); context.BindBuffersBase = internalLoadAPI<glBindBuffersBase>(contextPtr->BindBuffersBase);
            contextPtr->BindBuffersRange = load("glBindBuffersRange"); context.BindBuffersRange = internalLoadAPI<glBindBuffersRange>(contextPtr->BindBuffersRange);
            contextPtr->BindImageTextures = load("glBindImageTextures"); context.BindImageTextures = internalLoadAPI<glBindImageTextures>(contextPtr->BindImageTextures);
            contextPtr->BindSamplers = load("glBindSamplers"); context.BindSamplers = internalLoadAPI<glBindSamplers>(contextPtr->BindSamplers);
            contextPtr->BindTextures = load("glBindTextures"); context.BindTextures = internalLoadAPI<glBindTextures>(contextPtr->BindTextures);
            contextPtr->BindVertexBuffers = load("glBindVertexBuffers"); context.BindVertexBuffers = internalLoadAPI<glBindVertexBuffers>(contextPtr->BindVertexBuffers);
            contextPtr->BufferStorage = load("glBufferStorage"); context.BufferStorage = internalLoadAPI<glBufferStorage>(contextPtr->BufferStorage);
            contextPtr->ClearTexImage = load("glClearTexImage"); context.ClearTexImage = internalLoadAPI<glClearTexImage>(contextPtr->ClearTexImage);
            contextPtr->ClearTexSubImage = load("glClearTexSubImage"); context.ClearTexSubImage = internalLoadAPI<glClearTexSubImage>(contextPtr->ClearTexSubImage);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.4 APIs");
        }

        internal static unsafe void LoadGLcore45(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core45)
            {
                InternalIO.InternalAppendLog("The core version 4.5 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->BindTextureUnit = load("glBindTextureUnit"); context.BindTextureUnit = internalLoadAPI<glBindTextureUnit>(contextPtr->BindTextureUnit);
            contextPtr->BlitNamedFramebuffer = load("glBlitNamedFramebuffer"); context.BlitNamedFramebuffer = internalLoadAPI<glBlitNamedFramebuffer>(contextPtr->BlitNamedFramebuffer);
            contextPtr->CheckNamedFramebufferStatus = load("glCheckNamedFramebufferStatus"); context.CheckNamedFramebufferStatus = internalLoadAPI<glCheckNamedFramebufferStatus>(contextPtr->CheckNamedFramebufferStatus);
            contextPtr->ClearNamedBufferData = load("glClearNamedBufferData"); context.ClearNamedBufferData = internalLoadAPI<glClearNamedBufferData>(contextPtr->ClearNamedBufferData);
            contextPtr->ClearNamedBufferSubData = load("glClearNamedBufferSubData"); context.ClearNamedBufferSubData = internalLoadAPI<glClearNamedBufferSubData>(contextPtr->ClearNamedBufferSubData);
            contextPtr->ClearNamedFramebufferfi = load("glClearNamedFramebufferfi"); context.ClearNamedFramebufferfi = internalLoadAPI<glClearNamedFramebufferfi>(contextPtr->ClearNamedFramebufferfi);
            contextPtr->ClearNamedFramebufferfv = load("glClearNamedFramebufferfv"); context.ClearNamedFramebufferfv = internalLoadAPI<glClearNamedFramebufferfv>(contextPtr->ClearNamedFramebufferfv);
            contextPtr->ClearNamedFramebufferiv = load("glClearNamedFramebufferiv"); context.ClearNamedFramebufferiv = internalLoadAPI<glClearNamedFramebufferiv>(contextPtr->ClearNamedFramebufferiv);
            contextPtr->ClearNamedFramebufferuiv = load("glClearNamedFramebufferuiv"); context.ClearNamedFramebufferuiv = internalLoadAPI<glClearNamedFramebufferuiv>(contextPtr->ClearNamedFramebufferuiv);
            contextPtr->ClipControl = load("glClipControl"); context.ClipControl = internalLoadAPI<glClipControl>(contextPtr->ClipControl);
            contextPtr->CompressedTextureSubImage1D = load("glCompressedTextureSubImage1D"); context.CompressedTextureSubImage1D = internalLoadAPI<glCompressedTextureSubImage1D>(contextPtr->CompressedTextureSubImage1D);
            contextPtr->CompressedTextureSubImage2D = load("glCompressedTextureSubImage2D"); context.CompressedTextureSubImage2D = internalLoadAPI<glCompressedTextureSubImage2D>(contextPtr->CompressedTextureSubImage2D);
            contextPtr->CompressedTextureSubImage3D = load("glCompressedTextureSubImage3D"); context.CompressedTextureSubImage3D = internalLoadAPI<glCompressedTextureSubImage3D>(contextPtr->CompressedTextureSubImage3D);
            contextPtr->CopyNamedBufferSubData = load("glCopyNamedBufferSubData"); context.CopyNamedBufferSubData = internalLoadAPI<glCopyNamedBufferSubData>(contextPtr->CopyNamedBufferSubData);
            contextPtr->CopyTextureSubImage1D = load("glCopyTextureSubImage1D"); context.CopyTextureSubImage1D = internalLoadAPI<glCopyTextureSubImage1D>(contextPtr->CopyTextureSubImage1D);
            contextPtr->CopyTextureSubImage2D = load("glCopyTextureSubImage2D"); context.CopyTextureSubImage2D = internalLoadAPI<glCopyTextureSubImage2D>(contextPtr->CopyTextureSubImage2D);
            contextPtr->CopyTextureSubImage3D = load("glCopyTextureSubImage3D"); context.CopyTextureSubImage3D = internalLoadAPI<glCopyTextureSubImage3D>(contextPtr->CopyTextureSubImage3D);
            contextPtr->CreateBuffers = load("glCreateBuffers"); context.CreateBuffers = internalLoadAPI<glCreateBuffers>(contextPtr->CreateBuffers);
            contextPtr->CreateFramebuffers = load("glCreateFramebuffers"); context.CreateFramebuffers = internalLoadAPI<glCreateFramebuffers>(contextPtr->CreateFramebuffers);
            contextPtr->CreateProgramPipelines = load("glCreateProgramPipelines"); context.CreateProgramPipelines = internalLoadAPI<glCreateProgramPipelines>(contextPtr->CreateProgramPipelines);
            contextPtr->CreateQueries = load("glCreateQueries"); context.CreateQueries = internalLoadAPI<glCreateQueries>(contextPtr->CreateQueries);
            contextPtr->CreateRenderbuffers = load("glCreateRenderbuffers"); context.CreateRenderbuffers = internalLoadAPI<glCreateRenderbuffers>(contextPtr->CreateRenderbuffers);
            contextPtr->CreateSamplers = load("glCreateSamplers"); context.CreateSamplers = internalLoadAPI<glCreateSamplers>(contextPtr->CreateSamplers);
            contextPtr->CreateTextures = load("glCreateTextures"); context.CreateTextures = internalLoadAPI<glCreateTextures>(contextPtr->CreateTextures);
            contextPtr->CreateTransformFeedbacks = load("glCreateTransformFeedbacks"); context.CreateTransformFeedbacks = internalLoadAPI<glCreateTransformFeedbacks>(contextPtr->CreateTransformFeedbacks);
            contextPtr->CreateVertexArrays = load("glCreateVertexArrays"); context.CreateVertexArrays = internalLoadAPI<glCreateVertexArrays>(contextPtr->CreateVertexArrays);
            contextPtr->DisableVertexArrayAttrib = load("glDisableVertexArrayAttrib"); context.DisableVertexArrayAttrib = internalLoadAPI<glDisableVertexArrayAttrib>(contextPtr->DisableVertexArrayAttrib);
            contextPtr->EnableVertexArrayAttrib = load("glEnableVertexArrayAttrib"); context.EnableVertexArrayAttrib = internalLoadAPI<glEnableVertexArrayAttrib>(contextPtr->EnableVertexArrayAttrib);
            contextPtr->FlushMappedNamedBufferRange = load("glFlushMappedNamedBufferRange"); context.FlushMappedNamedBufferRange = internalLoadAPI<glFlushMappedNamedBufferRange>(contextPtr->FlushMappedNamedBufferRange);
            contextPtr->GenerateTextureMipmap = load("glGenerateTextureMipmap"); context.GenerateTextureMipmap = internalLoadAPI<glGenerateTextureMipmap>(contextPtr->GenerateTextureMipmap);
            contextPtr->GetCompressedTextureImage = load("glGetCompressedTextureImage"); context.GetCompressedTextureImage = internalLoadAPI<glGetCompressedTextureImage>(contextPtr->GetCompressedTextureImage);
            contextPtr->GetCompressedTextureSubImage = load("glGetCompressedTextureSubImage"); context.GetCompressedTextureSubImage = internalLoadAPI<glGetCompressedTextureSubImage>(contextPtr->GetCompressedTextureSubImage);
            contextPtr->GetGraphicsResetStatus = load("glGetGraphicsResetStatus"); context.GetGraphicsResetStatus = internalLoadAPI<glGetGraphicsResetStatus>(contextPtr->GetGraphicsResetStatus);
            contextPtr->GetNamedBufferParameteri64v = load("glGetNamedBufferParameteri64v"); context.GetNamedBufferParameteri64v = internalLoadAPI<glGetNamedBufferParameteri64v>(contextPtr->GetNamedBufferParameteri64v);
            contextPtr->GetNamedBufferParameteriv = load("glGetNamedBufferParameteriv"); context.GetNamedBufferParameteriv = internalLoadAPI<glGetNamedBufferParameteriv>(contextPtr->GetNamedBufferParameteriv);
            contextPtr->GetNamedBufferPointerv = load("glGetNamedBufferPointerv"); context.GetNamedBufferPointerv = internalLoadAPI<glGetNamedBufferPointerv>(contextPtr->GetNamedBufferPointerv);
            contextPtr->GetNamedBufferSubData = load("glGetNamedBufferSubData"); context.GetNamedBufferSubData = internalLoadAPI<glGetNamedBufferSubData>(contextPtr->GetNamedBufferSubData);
            contextPtr->GetNamedFramebufferAttachmentParameteriv = load("glGetNamedFramebufferAttachmentParameteriv"); context.GetNamedFramebufferAttachmentParameteriv = internalLoadAPI<glGetNamedFramebufferAttachmentParameteriv>(contextPtr->GetNamedFramebufferAttachmentParameteriv);
            contextPtr->GetNamedFramebufferParameteriv = load("glGetNamedFramebufferParameteriv"); context.GetNamedFramebufferParameteriv = internalLoadAPI<glGetNamedFramebufferParameteriv>(contextPtr->GetNamedFramebufferParameteriv);
            contextPtr->GetNamedRenderbufferParameteriv = load("glGetNamedRenderbufferParameteriv"); context.GetNamedRenderbufferParameteriv = internalLoadAPI<glGetNamedRenderbufferParameteriv>(contextPtr->GetNamedRenderbufferParameteriv);
            contextPtr->GetQueryBufferObjecti64v = load("glGetQueryBufferObjecti64v"); context.GetQueryBufferObjecti64v = internalLoadAPI<glGetQueryBufferObjecti64v>(contextPtr->GetQueryBufferObjecti64v);
            contextPtr->GetQueryBufferObjectiv = load("glGetQueryBufferObjectiv"); context.GetQueryBufferObjectiv = internalLoadAPI<glGetQueryBufferObjectiv>(contextPtr->GetQueryBufferObjectiv);
            contextPtr->GetQueryBufferObjectui64v = load("glGetQueryBufferObjectui64v"); context.GetQueryBufferObjectui64v = internalLoadAPI<glGetQueryBufferObjectui64v>(contextPtr->GetQueryBufferObjectui64v);
            contextPtr->GetQueryBufferObjectuiv = load("glGetQueryBufferObjectuiv"); context.GetQueryBufferObjectuiv = internalLoadAPI<glGetQueryBufferObjectuiv>(contextPtr->GetQueryBufferObjectuiv);
            contextPtr->GetTextureImage = load("glGetTextureImage"); context.GetTextureImage = internalLoadAPI<glGetTextureImage>(contextPtr->GetTextureImage);
            contextPtr->GetTextureLevelParameterfv = load("glGetTextureLevelParameterfv"); context.GetTextureLevelParameterfv = internalLoadAPI<glGetTextureLevelParameterfv>(contextPtr->GetTextureLevelParameterfv);
            contextPtr->GetTextureLevelParameteriv = load("glGetTextureLevelParameteriv"); context.GetTextureLevelParameteriv = internalLoadAPI<glGetTextureLevelParameteriv>(contextPtr->GetTextureLevelParameteriv);
            contextPtr->GetTextureParameterIiv = load("glGetTextureParameterIiv"); context.GetTextureParameterIiv = internalLoadAPI<glGetTextureParameterIiv>(contextPtr->GetTextureParameterIiv);
            contextPtr->GetTextureParameterIuiv = load("glGetTextureParameterIuiv"); context.GetTextureParameterIuiv = internalLoadAPI<glGetTextureParameterIuiv>(contextPtr->GetTextureParameterIuiv);
            contextPtr->GetTextureParameterfv = load("glGetTextureParameterfv"); context.GetTextureParameterfv = internalLoadAPI<glGetTextureParameterfv>(contextPtr->GetTextureParameterfv);
            contextPtr->GetTextureParameteriv = load("glGetTextureParameteriv"); context.GetTextureParameteriv = internalLoadAPI<glGetTextureParameteriv>(contextPtr->GetTextureParameteriv);
            contextPtr->GetTextureSubImage = load("glGetTextureSubImage"); context.GetTextureSubImage = internalLoadAPI<glGetTextureSubImage>(contextPtr->GetTextureSubImage);
            contextPtr->GetTransformFeedbacki64_v = load("glGetTransformFeedbacki64_v"); context.GetTransformFeedbacki64_v = internalLoadAPI<glGetTransformFeedbacki64_v>(contextPtr->GetTransformFeedbacki64_v);
            contextPtr->GetTransformFeedbacki_v = load("glGetTransformFeedbacki_v"); context.GetTransformFeedbacki_v = internalLoadAPI<glGetTransformFeedbacki_v>(contextPtr->GetTransformFeedbacki_v);
            contextPtr->GetTransformFeedbackiv = load("glGetTransformFeedbackiv"); context.GetTransformFeedbackiv = internalLoadAPI<glGetTransformFeedbackiv>(contextPtr->GetTransformFeedbackiv);
            contextPtr->GetVertexArrayIndexed64iv = load("glGetVertexArrayIndexed64iv"); context.GetVertexArrayIndexed64iv = internalLoadAPI<glGetVertexArrayIndexed64iv>(contextPtr->GetVertexArrayIndexed64iv);
            contextPtr->GetVertexArrayIndexediv = load("glGetVertexArrayIndexediv"); context.GetVertexArrayIndexediv = internalLoadAPI<glGetVertexArrayIndexediv>(contextPtr->GetVertexArrayIndexediv);
            contextPtr->GetVertexArrayiv = load("glGetVertexArrayiv"); context.GetVertexArrayiv = internalLoadAPI<glGetVertexArrayiv>(contextPtr->GetVertexArrayiv);
            contextPtr->GetnCompressedTexImage = load("glGetnCompressedTexImage"); context.GetnCompressedTexImage = internalLoadAPI<glGetnCompressedTexImage>(contextPtr->GetnCompressedTexImage);
            contextPtr->GetnTexImage = load("glGetnTexImage"); context.GetnTexImage = internalLoadAPI<glGetnTexImage>(contextPtr->GetnTexImage);
            contextPtr->GetnUniformdv = load("glGetnUniformdv"); context.GetnUniformdv = internalLoadAPI<glGetnUniformdv>(contextPtr->GetnUniformdv);
            contextPtr->GetnUniformfv = load("glGetnUniformfv"); context.GetnUniformfv = internalLoadAPI<glGetnUniformfv>(contextPtr->GetnUniformfv);
            contextPtr->GetnUniformiv = load("glGetnUniformiv"); context.GetnUniformiv = internalLoadAPI<glGetnUniformiv>(contextPtr->GetnUniformiv);
            contextPtr->GetnUniformuiv = load("glGetnUniformuiv"); context.GetnUniformuiv = internalLoadAPI<glGetnUniformuiv>(contextPtr->GetnUniformuiv);
            contextPtr->InvalidateNamedFramebufferData = load("glInvalidateNamedFramebufferData"); context.InvalidateNamedFramebufferData = internalLoadAPI<glInvalidateNamedFramebufferData>(contextPtr->InvalidateNamedFramebufferData);
            contextPtr->InvalidateNamedFramebufferSubData = load("glInvalidateNamedFramebufferSubData"); context.InvalidateNamedFramebufferSubData = internalLoadAPI<glInvalidateNamedFramebufferSubData>(contextPtr->InvalidateNamedFramebufferSubData);
            contextPtr->MapNamedBuffer = load("glMapNamedBuffer"); context.MapNamedBuffer = internalLoadAPI<glMapNamedBuffer>(contextPtr->MapNamedBuffer);
            contextPtr->MapNamedBufferRange = load("glMapNamedBufferRange"); context.MapNamedBufferRange = internalLoadAPI<glMapNamedBufferRange>(contextPtr->MapNamedBufferRange);
            contextPtr->MemoryBarrierByRegion = load("glMemoryBarrierByRegion"); context.MemoryBarrierByRegion = internalLoadAPI<glMemoryBarrierByRegion>(contextPtr->MemoryBarrierByRegion);
            contextPtr->NamedBufferData = load("glNamedBufferData"); context.NamedBufferData = internalLoadAPI<glNamedBufferData>(contextPtr->NamedBufferData);
            contextPtr->NamedBufferStorage = load("glNamedBufferStorage"); context.NamedBufferStorage = internalLoadAPI<glNamedBufferStorage>(contextPtr->NamedBufferStorage);
            contextPtr->NamedBufferSubData = load("glNamedBufferSubData"); context.NamedBufferSubData = internalLoadAPI<glNamedBufferSubData>(contextPtr->NamedBufferSubData);
            contextPtr->NamedFramebufferDrawBuffer = load("glNamedFramebufferDrawBuffer"); context.NamedFramebufferDrawBuffer = internalLoadAPI<glNamedFramebufferDrawBuffer>(contextPtr->NamedFramebufferDrawBuffer);
            contextPtr->NamedFramebufferDrawBuffers = load("glNamedFramebufferDrawBuffers"); context.NamedFramebufferDrawBuffers = internalLoadAPI<glNamedFramebufferDrawBuffers>(contextPtr->NamedFramebufferDrawBuffers);
            contextPtr->NamedFramebufferParameteri = load("glNamedFramebufferParameteri"); context.NamedFramebufferParameteri = internalLoadAPI<glNamedFramebufferParameteri>(contextPtr->NamedFramebufferParameteri);
            contextPtr->NamedFramebufferReadBuffer = load("glNamedFramebufferReadBuffer"); context.NamedFramebufferReadBuffer = internalLoadAPI<glNamedFramebufferReadBuffer>(contextPtr->NamedFramebufferReadBuffer);
            contextPtr->NamedFramebufferRenderbuffer = load("glNamedFramebufferRenderbuffer"); context.NamedFramebufferRenderbuffer = internalLoadAPI<glNamedFramebufferRenderbuffer>(contextPtr->NamedFramebufferRenderbuffer);
            contextPtr->NamedFramebufferTexture = load("glNamedFramebufferTexture"); context.NamedFramebufferTexture = internalLoadAPI<glNamedFramebufferTexture>(contextPtr->NamedFramebufferTexture);
            contextPtr->NamedFramebufferTextureLayer = load("glNamedFramebufferTextureLayer"); context.NamedFramebufferTextureLayer = internalLoadAPI<glNamedFramebufferTextureLayer>(contextPtr->NamedFramebufferTextureLayer);
            contextPtr->NamedRenderbufferStorage = load("glNamedRenderbufferStorage"); context.NamedRenderbufferStorage = internalLoadAPI<glNamedRenderbufferStorage>(contextPtr->NamedRenderbufferStorage);
            contextPtr->NamedRenderbufferStorageMultisample = load("glNamedRenderbufferStorageMultisample"); context.NamedRenderbufferStorageMultisample = internalLoadAPI<glNamedRenderbufferStorageMultisample>(contextPtr->NamedRenderbufferStorageMultisample);
            contextPtr->ReadnPixels = load("glReadnPixels"); context.ReadnPixels = internalLoadAPI<glReadnPixels>(contextPtr->ReadnPixels);
            contextPtr->TextureBarrier = load("glTextureBarrier"); context.TextureBarrier = internalLoadAPI<glTextureBarrier>(contextPtr->TextureBarrier);
            contextPtr->TextureBuffer = load("glTextureBuffer"); context.TextureBuffer = internalLoadAPI<glTextureBuffer>(contextPtr->TextureBuffer);
            contextPtr->TextureBufferRange = load("glTextureBufferRange"); context.TextureBufferRange = internalLoadAPI<glTextureBufferRange>(contextPtr->TextureBufferRange);
            contextPtr->TextureParameterIiv = load("glTextureParameterIiv"); context.TextureParameterIiv = internalLoadAPI<glTextureParameterIiv>(contextPtr->TextureParameterIiv);
            contextPtr->TextureParameterIuiv = load("glTextureParameterIuiv"); context.TextureParameterIuiv = internalLoadAPI<glTextureParameterIuiv>(contextPtr->TextureParameterIuiv);
            contextPtr->TextureParameterf = load("glTextureParameterf"); context.TextureParameterf = internalLoadAPI<glTextureParameterf>(contextPtr->TextureParameterf);
            contextPtr->TextureParameterfv = load("glTextureParameterfv"); context.TextureParameterfv = internalLoadAPI<glTextureParameterfv>(contextPtr->TextureParameterfv);
            contextPtr->TextureParameteri = load("glTextureParameteri"); context.TextureParameteri = internalLoadAPI<glTextureParameteri>(contextPtr->TextureParameteri);
            contextPtr->TextureParameteriv = load("glTextureParameteriv"); context.TextureParameteriv = internalLoadAPI<glTextureParameteriv>(contextPtr->TextureParameteriv);
            contextPtr->TextureStorage1D = load("glTextureStorage1D"); context.TextureStorage1D = internalLoadAPI<glTextureStorage1D>(contextPtr->TextureStorage1D);
            contextPtr->TextureStorage2D = load("glTextureStorage2D"); context.TextureStorage2D = internalLoadAPI<glTextureStorage2D>(contextPtr->TextureStorage2D);
            contextPtr->TextureStorage2DMultisample = load("glTextureStorage2DMultisample"); context.TextureStorage2DMultisample = internalLoadAPI<glTextureStorage2DMultisample>(contextPtr->TextureStorage2DMultisample);
            contextPtr->TextureStorage3D = load("glTextureStorage3D"); context.TextureStorage3D = internalLoadAPI<glTextureStorage3D>(contextPtr->TextureStorage3D);
            contextPtr->TextureStorage3DMultisample = load("glTextureStorage3DMultisample"); context.TextureStorage3DMultisample = internalLoadAPI<glTextureStorage3DMultisample>(contextPtr->TextureStorage3DMultisample);
            contextPtr->TextureSubImage1D = load("glTextureSubImage1D"); context.TextureSubImage1D = internalLoadAPI<glTextureSubImage1D>(contextPtr->TextureSubImage1D);
            contextPtr->TextureSubImage2D = load("glTextureSubImage2D"); context.TextureSubImage2D = internalLoadAPI<glTextureSubImage2D>(contextPtr->TextureSubImage2D);
            contextPtr->TextureSubImage3D = load("glTextureSubImage3D"); context.TextureSubImage3D = internalLoadAPI<glTextureSubImage3D>(contextPtr->TextureSubImage3D);
            contextPtr->TransformFeedbackBufferBase = load("glTransformFeedbackBufferBase"); context.TransformFeedbackBufferBase = internalLoadAPI<glTransformFeedbackBufferBase>(contextPtr->TransformFeedbackBufferBase);
            contextPtr->TransformFeedbackBufferRange = load("glTransformFeedbackBufferRange"); context.TransformFeedbackBufferRange = internalLoadAPI<glTransformFeedbackBufferRange>(contextPtr->TransformFeedbackBufferRange);
            contextPtr->UnmapNamedBuffer = load("glUnmapNamedBuffer"); context.UnmapNamedBuffer = internalLoadAPI<glUnmapNamedBuffer>(contextPtr->UnmapNamedBuffer);
            contextPtr->VertexArrayAttribBinding = load("glVertexArrayAttribBinding"); context.VertexArrayAttribBinding = internalLoadAPI<glVertexArrayAttribBinding>(contextPtr->VertexArrayAttribBinding);
            contextPtr->VertexArrayAttribFormat = load("glVertexArrayAttribFormat"); context.VertexArrayAttribFormat = internalLoadAPI<glVertexArrayAttribFormat>(contextPtr->VertexArrayAttribFormat);
            contextPtr->VertexArrayAttribIFormat = load("glVertexArrayAttribIFormat"); context.VertexArrayAttribIFormat = internalLoadAPI<glVertexArrayAttribIFormat>(contextPtr->VertexArrayAttribIFormat);
            contextPtr->VertexArrayAttribLFormat = load("glVertexArrayAttribLFormat"); context.VertexArrayAttribLFormat = internalLoadAPI<glVertexArrayAttribLFormat>(contextPtr->VertexArrayAttribLFormat);
            contextPtr->VertexArrayBindingDivisor = load("glVertexArrayBindingDivisor"); context.VertexArrayBindingDivisor = internalLoadAPI<glVertexArrayBindingDivisor>(contextPtr->VertexArrayBindingDivisor);
            contextPtr->VertexArrayElementBuffer = load("glVertexArrayElementBuffer"); context.VertexArrayElementBuffer = internalLoadAPI<glVertexArrayElementBuffer>(contextPtr->VertexArrayElementBuffer);
            contextPtr->VertexArrayVertexBuffer = load("glVertexArrayVertexBuffer"); context.VertexArrayVertexBuffer = internalLoadAPI<glVertexArrayVertexBuffer>(contextPtr->VertexArrayVertexBuffer);
            contextPtr->VertexArrayVertexBuffers = load("glVertexArrayVertexBuffers"); context.VertexArrayVertexBuffers = internalLoadAPI<glVertexArrayVertexBuffers>(contextPtr->VertexArrayVertexBuffers);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.5 APIs");
        }

        internal static unsafe void LoadGLcore46(GLcontextIO* contextPtr, GLcontext context, GLfuncLoader load)
        {
            if (!core46)
            {
                InternalIO.InternalAppendLog("The core version 4.6 is not supported.");
                throw new Exception("Final Version");
            }
            #region load
            contextPtr->MultiDrawArraysIndirectCount = load("glMultiDrawArraysIndirectCount"); context.MultiDrawArraysIndirectCount = internalLoadAPI<glMultiDrawArraysIndirectCount>(contextPtr->MultiDrawArraysIndirectCount);
            contextPtr->MultiDrawElementsIndirectCount = load("glMultiDrawElementsIndirectCount"); context.MultiDrawElementsIndirectCount = internalLoadAPI<glMultiDrawElementsIndirectCount>(contextPtr->MultiDrawElementsIndirectCount);
            contextPtr->PolygonOffsetClamp = load("glPolygonOffsetClamp"); context.PolygonOffsetClamp = internalLoadAPI<glPolygonOffsetClamp>(contextPtr->PolygonOffsetClamp);
            contextPtr->SpecializeShader = load("glSpecializeShader"); context.SpecializeShader = internalLoadAPI<glSpecializeShader>(contextPtr->SpecializeShader);

            #endregion
            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.6 APIs");
        }
    }

}
