//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glApiManager.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Math;

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

        private const int glExtension = 0x1f03;
        private const int glVersion = 0x1f02;

        private static readonly string[] glBranch =
            new string[] {
                "OpenGL ES-CM ",
                "OpenGL ES-CL ",
                "OpenGL ES ",
                "OpenGL SC "
            };

        private static string glExtensionList;

        /// <summary>
        /// Load all aupportable OpenGL APIs and bind them to given <see cref="Form"/> context.
        /// </summary>
        /// <param name="form">The <see cref="Form"/> used as current OpenGL context</param>
        /// <param name="loader">A delegate to get address of OprnGL api</param>
        public static unsafe void LoadGLapi(this Form form, GLfuncLoader loader)
        {
            GLcontext* context = (GLcontext*)form.graphicContextID;
            if (loader == null)
            {
                loader = InternalIO.glfwGetProcAddress;
            }
#pragma warning disable CS8909
            if (((GLcontext*)form.graphicContextID)->glGetString != (delegate*<uint, IntPtr>)IntPtr.Zero)
#pragma warning restore CS8909
            {
                return;
            }
            IntPtr proc = loader("glGetString");
            if (proc == IntPtr.Zero)
            {
#if DEBUG
                Console.WriteLine("Cannot load GL.");
#endif
                throw new Exception("Cannot load gl");
            }
            context->glGetString = (delegate*<uint, IntPtr>)proc;

            string coreVersion;
            if (context->glGetString(glVersion) == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog("Failed to call glGetString", LogType.Error);
            }
            coreVersion = Marshal.PtrToStringAnsi(context->glGetString(glVersion));

            foreach (string branch in glBranch)
            {
                if (coreVersion.Contains(branch))
                {
                    coreVersion.Replace(branch, string.Empty);
                }
            }
            string[] versionSep = coreVersion.Split('.');
            int majorVersion = int.Parse(versionSep[0]);
            int minorVersion = int.Parse(versionSep[1]);
            CheckCoreVersion(majorVersion, minorVersion);

            try
            {
                #region load_core_gl

                LoadGLcore10((GLcontext*)form.graphicContextID, loader);
                LoadGLcore11((GLcontext*)form.graphicContextID, loader);
                LoadGLcore12((GLcontext*)form.graphicContextID, loader);
                LoadGLcore13((GLcontext*)form.graphicContextID, loader);
                LoadGLcore14((GLcontext*)form.graphicContextID, loader);
                LoadGLcore15((GLcontext*)form.graphicContextID, loader);
                LoadGLcore20((GLcontext*)form.graphicContextID, loader);
                LoadGLcore21((GLcontext*)form.graphicContextID, loader);
                LoadGLcore30((GLcontext*)form.graphicContextID, loader);
                LoadGLcore31((GLcontext*)form.graphicContextID, loader);
                LoadGLcore32((GLcontext*)form.graphicContextID, loader);
                LoadGLcore33((GLcontext*)form.graphicContextID, loader);
                LoadGLcore40((GLcontext*)form.graphicContextID, loader);
                LoadGLcore41((GLcontext*)form.graphicContextID, loader);
                LoadGLcore42((GLcontext*)form.graphicContextID, loader);
                LoadGLcore43((GLcontext*)form.graphicContextID, loader);
                LoadGLcore44((GLcontext*)form.graphicContextID, loader);
                LoadGLcore45((GLcontext*)form.graphicContextID, loader);
                LoadGLcore46((GLcontext*)form.graphicContextID, loader);

                #endregion load_core_gl
            }
            catch (Exception ex)
            {
                if (ex.Message != "Final Version")
                {
                    throw;
                }
            }

            IntPtr strPtr = context->glGetString(glExtension);
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

            List<string> extensions =
                extension.Split(" ").ToList();

            #region checkGLextensions

            if (extensions.Contains("gl_4DFX_multisample")) { _4DFX_multisample = true; extensions.Remove("gl_4DFX_multisample"); }
            if (extensions.Contains("gl_3DFX_tbuffer")) { _3DFX_tbuffer = true; extensions.Remove("gl_3DFX_tbuffer"); }
            if (extensions.Contains("gl_3DFX_texture_compression_FXT1")) { _3DFX_texture_compression_FXT1 = true; extensions.Remove("gl_3DFX_texture_compression_FXT1"); }
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

            #endregion checkGLextensions

            InternalIO.InternalWriteLog("Find OpenGL extensions.", LogType.Info);
        }

        /// <summary>
        /// Get the function pointer of given OpenGL api name
        /// </summary>
        /// <param name="name">Name of the OpenGL api</param>
        /// <returns>The funtion pointer of the api in form of <see cref="IntPtr"/></returns>
        public static IntPtr LoadGLfunc(string name)
        {
            IntPtr ret = InternalIO.glfwGetProcAddress(name);
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
            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 0)))
            {
                core10 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 1)))
            {
                core11 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 2)))
            {
                core12 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 3)))
            {
                core13 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 4)))
            {
                core14 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 1) || ((majorVersion == 1) && (minorVersion == 5)))
            {
                core15 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 2) || ((majorVersion == 2) && (minorVersion == 0)))
            {
                core20 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 2) || ((majorVersion == 2) && (minorVersion == 1)))
            {
                core21 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 3) || ((majorVersion == 3) && (minorVersion == 0)))
            {
                core30 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 3) || ((majorVersion == 3) && (minorVersion == 1)))
            {
                core31 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 3) || ((majorVersion == 3) && (minorVersion == 2)))
            {
                core32 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 3) || ((majorVersion == 3) && (minorVersion == 3)))
            {
                core33 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 0)))
            {
                core40 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 1)))
            {
                core41 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 2)))
            {
                core42 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 3)))
            {
                core43 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 4)))
            {
                core44 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 5)))
            {
                core45 = true;
            }
            else
            {
                goto log;
            }

            if ((majorVersion >= 4) || ((majorVersion == 4) && (minorVersion == 6)))
            {
                core46 = true;
            }
            else
            {
                goto log;
            }

        log:
            {
                InternalIO.InternalWriteLog("Find core OpenGL version.", LogType.Info);
                return;
            }
        }

        internal static unsafe string GetExtensions(this GLcontext context)
        {
            if (context.glGetString == null)
            {
                return string.Empty;
            }
            return Marshal.PtrToStringAnsi(
                context.glGetString(glExtension));
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
                    $"{$"Failed to convert the api {typeof(T).Name}.\n"}{ex.Message}", LogType.Warning);
                return default;
            }
        }

        internal static unsafe void LoadGLcore10(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core10)
            {
                InternalIO.InternalAppendLog("The core version 1.0 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBlendFunc = (delegate*<uint, uint, void>)load("glBlendFunc");
            contextPtr->glClear = (delegate*<uint, void>)load("glClear");
            contextPtr->glClearColor = (delegate*<float, float, float, float, void>)load("glClearColor");
            contextPtr->glClearDepth = (delegate*<double, void>)load("glClearDepth");
            contextPtr->glClearStencil = (delegate*<int, void>)load("glClearStencil");
            contextPtr->glColorMask = (delegate*<bool, bool, bool, bool, void>)load("glColorMask");
            contextPtr->glCullFace = (delegate*<uint, void>)load("glCullFace");
            contextPtr->glDepthFunc = (delegate*<uint, void>)load("glDepthFunc");
            contextPtr->glDepthMask = (delegate*<bool, void>)load("glDepthMask");
            contextPtr->glDepthRange = (delegate*<double, double, void>)load("glDepthRange");
            contextPtr->glDisable = (delegate*<uint, void>)load("glDisable");
            contextPtr->glDrawBuffer = (delegate*<uint, void>)load("glDrawBuffer");
            contextPtr->glEnable = (delegate*<uint, void>)load("glEnable");
            contextPtr->glFinish = (delegate*<void>)load("glFinish");
            contextPtr->glFlush = (delegate*<void>)load("glFlush");
            contextPtr->glFrontFace = (delegate*<uint, void>)load("glFrontFace");
            contextPtr->glGetBooleanv = (delegate*<uint, bool*, void>)load("glGetBooleanv");
            contextPtr->glGetDoublev = (delegate*<uint, double*, void>)load("glGetDoublev");
            contextPtr->glGetError = (delegate*<uint>)load("glGetError");
            contextPtr->glGetFloatv = (delegate*<uint, float*, void>)load("glGetFloatv");
            contextPtr->glGetIntegerv = (delegate*<uint, int*, void>)load("glGetIntegerv");
            contextPtr->glGetString = (delegate*<uint, IntPtr>)load("glGetString");
            contextPtr->glGetTexImage = (delegate*<uint, int, uint, uint, void*, void>)load("glGetTexImage");
            contextPtr->glGetTexLevelParameterfv = (delegate*<uint, int, uint, float*, void>)load("glGetTexLevelParameterfv");
            contextPtr->glGetTexLevelParameteriv = (delegate*<uint, int, uint, int*, void>)load("glGetTexLevelParameteriv");
            contextPtr->glGetTexParameterfv = (delegate*<uint, uint, float*, void>)load("glGetTexParameterfv");
            contextPtr->glGetTexParameteriv = (delegate*<uint, uint, int*, void>)load("glGetTexParameteriv");
            contextPtr->glHint = (delegate*<uint, uint, void>)load("glHint");
            contextPtr->glIsEnabled = (delegate*<uint, bool>)load("glIsEnabled");
            contextPtr->glLineWidth = (delegate*<float, void>)load("glLineWidth");
            contextPtr->glLogicOp = (delegate*<uint, void>)load("glLogicOp");
            contextPtr->glPixelStoref = (delegate*<uint, float, void>)load("glPixelStoref");
            contextPtr->glPixelStorei = (delegate*<uint, int, void>)load("glPixelStorei");
            contextPtr->glPointSize = (delegate*<float, void>)load("glPointSize");
            contextPtr->glPolygonMode = (delegate*<uint, uint, void>)load("glPolygonMode");
            contextPtr->glReadBuffer = (delegate*<uint, void>)load("glReadBuffer");
            contextPtr->glReadPixels = (delegate*<int, int, uint, uint, uint, uint, void*, void>)load("glReadPixels");
            contextPtr->glScissor = (delegate*<int, int, uint, uint, void>)load("glScissor");
            contextPtr->glStencilFunc = (delegate*<uint, int, uint, void>)load("glStencilFunc");
            contextPtr->glStencilMask = (delegate*<uint, void>)load("glStencilMask");
            contextPtr->glStencilOp = (delegate*<uint, uint, uint, void>)load("glStencilOp");
            contextPtr->glTexImage1D = (delegate*<uint, int, int, uint, int, uint, uint, void*, void>)load("glTexImage1D");
            contextPtr->glTexImage2D = (delegate*<uint, int, uint, uint, uint, int, uint, uint, void*, void>)load("glTexImage2D");
            contextPtr->glTexParameterf = (delegate*<uint, uint, float, void>)load("glTexParameterf");
            contextPtr->glTexParameterfv = (delegate*<uint, uint, float*, void>)load("glTexParameterfv");
            contextPtr->glTexParameteri = (delegate*<uint, uint, int, void>)load("glTexParameteri");
            contextPtr->glTexParameteriv = (delegate*<uint, uint, int*, void>)load("glTexParameteriv");
            contextPtr->glViewport = (delegate*<int, int, uint, uint, void>)load("glViewport");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.0 APIs");
        }

        internal static unsafe void LoadGLcore11(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core11)
            {
                InternalIO.InternalAppendLog("The core version 1.1 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindTexture = (delegate*<uint, uint, void>)load("glBindTexture");
            contextPtr->glCopyTexImage1D = (delegate*<uint, int, uint, int, int, uint, int, void>)load("glCopyTexImage1D");
            contextPtr->glCopyTexImage2D = (delegate*<uint, int, uint, int, int, uint, uint, int, void>)load("glCopyTexImage2D");
            contextPtr->glCopyTexSubImage1D = (delegate*<uint, int, int, int, int, uint, void>)load("glCopyTexSubImage1D");
            contextPtr->glCopyTexSubImage2D = (delegate*<uint, int, int, int, int, int, uint, uint, void>)load("glCopyTexSubImage2D");
            contextPtr->glDeleteTextures = (delegate*<uint, uint*, void>)load("glDeleteTextures");
            contextPtr->glDrawArrays = (delegate*<uint, int, uint, void>)load("glDrawArrays");
            contextPtr->glDrawElements = (delegate*<uint, uint, uint, void*, void>)load("glDrawElements");
            contextPtr->glGenTextures = (delegate*<uint, uint*, void>)load("glGenTextures");
            contextPtr->glGetPointerv = (delegate*<uint, void**, void>)load("glGetPointerv");
            contextPtr->glIsTexture = (delegate*<uint, bool>)load("glIsTexture");
            contextPtr->glPolygonOffset = (delegate*<float, float, void>)load("glPolygonOffset");
            contextPtr->glTexSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load("glTexSubImage1D");
            contextPtr->glTexSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load("glTexSubImage2D");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.1 APIs");
        }

        internal static unsafe void LoadGLcore12(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core12)
            {
                InternalIO.InternalAppendLog("The core version 1.2 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glCopyTexSubImage3D = (delegate*<uint, int, int, int, int, int, int, uint, uint, void>)load("glCopyTexSubImage3D");
            contextPtr->glDrawRangeElements = (delegate*<uint, uint, uint, uint, uint, void*, void>)load("glDrawRangeElements");
            contextPtr->glTexImage3D = (delegate*<uint, int, int, uint, uint, uint, int, uint, uint, void*, void>)load("glTexImage3D");
            contextPtr->glTexSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load("glTexSubImage3D");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.2 APIs");
        }

        internal static unsafe void LoadGLcore13(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core13)
            {
                InternalIO.InternalAppendLog("The core version 1.3 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glActiveTexture = (delegate*<uint, void>)load("glActiveTexture");
            contextPtr->glCompressedTexImage1D = (delegate*<uint, int, uint, uint, int, uint, void*, void>)load("glCompressedTexImage1D");
            contextPtr->glCompressedTexImage2D = (delegate*<uint, int, uint, uint, uint, int, uint, void*, void>)load("glCompressedTexImage2D");
            contextPtr->glCompressedTexImage3D = (delegate*<uint, int, uint, uint, uint, uint, int, uint, void*, void>)load("glCompressedTexImage3D");
            contextPtr->glCompressedTexSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load("glCompressedTexSubImage1D");
            contextPtr->glCompressedTexSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load("glCompressedTexSubImage2D");
            contextPtr->glCompressedTexSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load("glCompressedTexSubImage3D");
            contextPtr->glGetCompressedTexImage = (delegate*<uint, int, void*, void>)load("glGetCompressedTexImage");
            contextPtr->glSampleCoverage = (delegate*<float, bool, void>)load("glSampleCoverage");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.3 APIs");
        }

        internal static unsafe void LoadGLcore14(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core14)
            {
                InternalIO.InternalAppendLog("The core version 1.4 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBlendColor = (delegate*<float, float, float, float, void>)load("glBlendColor");
            contextPtr->glBlendEquation = (delegate*<uint, void>)load("glBlendEquation");
            contextPtr->glBlendFuncSeparate = (delegate*<uint, uint, uint, uint, void>)load("glBlendFuncSeparate");
            contextPtr->glMultiDrawArrays = (delegate*<uint, int*, uint*, uint, void>)load("glMultiDrawArrays");
            contextPtr->glMultiDrawElements = (delegate*<uint, uint*, uint, void*, void**, uint, void>)load("glMultiDrawElements");
            contextPtr->glPointParameterf = (delegate*<uint, float, void>)load("glPointParameterf");
            contextPtr->glPointParameterfv = (delegate*<uint, float*, void>)load("glPointParameterfv");
            contextPtr->glPointParameteri = (delegate*<uint, int, void>)load("glPointParameteri");
            contextPtr->glPointParameteriv = (delegate*<uint, int*, void>)load("glPointParameteriv");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.4 APIs");
        }

        internal static unsafe void LoadGLcore15(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core15)
            {
                InternalIO.InternalAppendLog("The core version 1.5 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBeginQuery = (delegate*<uint, uint, void>)load("glBeginQuery");
            contextPtr->glBindBuffer = (delegate*<uint, uint, void>)load("glBindBuffer");
            contextPtr->glBufferData = (delegate*<uint, IntPtr, IntPtr, uint, void>)load("glBufferData");
            contextPtr->glBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load("glBufferSubData");
            contextPtr->glDeleteBuffers = (delegate*<uint, uint*, void>)load("glDeleteBuffers");
            contextPtr->glDeleteQueries = (delegate*<uint, uint*, void>)load("glDeleteQueries");
            contextPtr->glEndQuery = (delegate*<uint, void>)load("glEndQuery");
            contextPtr->glGenBuffers = (delegate*<uint, uint*, void>)load("glGenBuffers");
            contextPtr->glGenQueries = (delegate*<uint, uint*, void>)load("glGenQueries");
            contextPtr->glGetBufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetBufferParameteriv");
            contextPtr->glGetBufferPointerv = (delegate*<uint, uint, void**, void>)load("glGetBufferPointerv");
            contextPtr->glGetBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load("glGetBufferSubData");
            contextPtr->glGetQueryObjectiv = (delegate*<uint, uint, int*, void>)load("glGetQueryObjectiv");
            contextPtr->glGetQueryObjectuiv = (delegate*<uint, uint, uint*, void>)load("glGetQueryObjectuiv");
            contextPtr->glGetQueryiv = (delegate*<uint, uint, int*, void>)load("glGetQueryiv");
            contextPtr->glIsBuffer = (delegate*<uint, bool>)load("glIsBuffer");
            contextPtr->glIsQuery = (delegate*<uint, bool>)load("glIsQuery");
            contextPtr->glMapBuffer = (delegate*<uint, uint, void*>)load("glMapBuffer");
            contextPtr->glUnmapBuffer = (delegate*<uint, bool>)load("glUnmapBuffer");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 1.5 APIs");
        }

        internal static unsafe void LoadGLcore20(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core20)
            {
                InternalIO.InternalAppendLog("The core version 2.0 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glAttachShader = (delegate*<uint, uint, void>)load("glAttachShader");
            contextPtr->glBindAttribLocation = (delegate*<uint, uint, byte*, void>)load("glBindAttribLocation");
            contextPtr->glBlendEquationSeparate = (delegate*<uint, uint, void>)load("glBlendEquationSeparate");
            contextPtr->glCompileShader = (delegate*<uint, void>)load("glCompileShader");
            contextPtr->glCreateProgram = (delegate*<uint>)load("glCreateProgram");
            contextPtr->glCreateShader = (delegate*<uint, uint>)load("glCreateShader");
            contextPtr->glDeleteProgram = (delegate*<uint, void>)load("glDeleteProgram");
            contextPtr->glDeleteShader = (delegate*<uint, void>)load("glDeleteShader");
            contextPtr->glDetachShader = (delegate*<uint, uint, void>)load("glDetachShader");
            contextPtr->glDisableVertexAttribArray = (delegate*<uint, void>)load("glDisableVertexAttribArray");
            contextPtr->glDrawBuffers = (delegate*<uint, uint*, void>)load("glDrawBuffers");
            contextPtr->glEnableVertexAttribArray = (delegate*<uint, void>)load("glEnableVertexAttribArray");
            contextPtr->glGetActiveAttrib = (delegate*<uint, uint, uint, uint*, int*, uint*, byte*, void>)load("glGetActiveAttrib");
            contextPtr->glGetActiveUniform = (delegate*<uint, uint, uint, uint*, int*, uint*, byte*, void>)load("glGetActiveUniform");
            contextPtr->glGetAttachedShaders = (delegate*<uint, uint, uint*, uint*, void>)load("glGetAttachedShaders");
            contextPtr->glGetAttribLocation = (delegate*<uint, byte*, int>)load("glGetAttribLocation");
            contextPtr->glGetProgramInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load("glGetProgramInfoLog");
            contextPtr->glGetProgramiv = (delegate*<uint, uint, int*, void>)load("glGetProgramiv");
            contextPtr->glGetShaderInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load("glGetShaderInfoLog");
            contextPtr->glGetShaderSource = (delegate*<uint, uint, uint*, byte*, void>)load("glGetShaderSource");
            contextPtr->glGetShaderiv = (delegate*<uint, uint, int*, void>)load("glGetShaderiv");
            contextPtr->glGetUniformLocation = (delegate*<uint, byte*, int>)load("glGetUniformLocation");
            contextPtr->glGetUniformfv = (delegate*<uint, int, float*, void>)load("glGetUniformfv");
            contextPtr->glGetUniformiv = (delegate*<uint, int, int*, void>)load("glGetUniformiv");
            contextPtr->glGetVertexAttribPointerv = (delegate*<uint, uint, void**, void>)load("glGetVertexAttribPointerv");
            contextPtr->glGetVertexAttribdv = (delegate*<uint, uint, double*, void>)load("glGetVertexAttribdv");
            contextPtr->glGetVertexAttribfv = (delegate*<uint, uint, float*, void>)load("glGetVertexAttribfv");
            contextPtr->glGetVertexAttribiv = (delegate*<uint, uint, int*, void>)load("glGetVertexAttribiv");
            contextPtr->glIsProgram = (delegate*<uint, bool>)load("glIsProgram");
            contextPtr->glIsShader = (delegate*<uint, bool>)load("glIsShader");
            contextPtr->glLinkProgram = (delegate*<uint, void>)load("glLinkProgram");
            contextPtr->glShaderSource = (delegate* unmanaged[Stdcall]<uint, uint, byte**, void*, void>)load("glShaderSource");
            contextPtr->glStencilFuncSeparate = (delegate*<uint, uint, int, uint, void>)load("glStencilFuncSeparate");
            contextPtr->glStencilMaskSeparate = (delegate*<uint, uint, void>)load("glStencilMaskSeparate");
            contextPtr->glStencilOpSeparate = (delegate*<uint, uint, uint, uint, void>)load("glStencilOpSeparate");
            contextPtr->glUniform1f = (delegate*<int, float, void>)load("glUniform1f");
            contextPtr->glUniform1fv = (delegate*<int, uint, Vec4d*, void>)load("glUniform1fv");
            contextPtr->glUniform1i = (delegate*<int, int, void>)load("glUniform1i");
            contextPtr->glUniform1iv = (delegate*<int, uint, int*, void>)load("glUniform1iv");
            contextPtr->glUniform2f = (delegate*<int, float, float, void>)load("glUniform2f");
            contextPtr->glUniform2fv = (delegate*<int, uint, float*, void>)load("glUniform2fv");
            contextPtr->glUniform2i = (delegate*<int, int, int, void>)load("glUniform2i");
            contextPtr->glUniform2iv = (delegate*<int, uint, int*, void>)load("glUniform2iv");
            contextPtr->glUniform3f = (delegate*<int, float, float, float, void>)load("glUniform3f");
            contextPtr->glUniform3fv = (delegate*<int, uint, float*, void>)load("glUniform3fv");
            contextPtr->glUniform3i = (delegate*<int, int, int, int, void>)load("glUniform3i");
            contextPtr->glUniform3iv = (delegate*<int, uint, int*, void>)load("glUniform3iv");
            contextPtr->glUniform4f = (delegate*<int, float, float, float, float, void>)load("glUniform4f");
            contextPtr->glUniform4fv = (delegate*<int, uint, float*, void>)load("glUniform4fv");
            contextPtr->glUniform4i = (delegate*<int, int, int, int, int, void>)load("glUniform4i");
            contextPtr->glUniform4iv = (delegate*<int, uint, int*, void>)load("glUniform4iv");
            contextPtr->glUniformMatrix2fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix2fv");
            contextPtr->glUniformMatrix3fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix3fv");
            contextPtr->glUniformMatrix4fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix4fv");
            contextPtr->glUseProgram = (delegate*<uint, void>)load("glUseProgram");
            contextPtr->glValidateProgram = (delegate*<uint, void>)load("glValidateProgram");
            contextPtr->glVertexAttrib1d = (delegate*<uint, double, void>)load("glVertexAttrib1d");
            contextPtr->glVertexAttrib1dv = (delegate*<uint, double*, void>)load("glVertexAttrib1dv");
            contextPtr->glVertexAttrib1f = (delegate*<uint, float, void>)load("glVertexAttrib1f");
            contextPtr->glVertexAttrib1fv = (delegate*<uint, float*, void>)load("glVertexAttrib1fv");
            contextPtr->glVertexAttrib1s = (delegate*<uint, short, void>)load("glVertexAttrib1s");
            contextPtr->glVertexAttrib1sv = (delegate*<uint, short*, void>)load("glVertexAttrib1sv");
            contextPtr->glVertexAttrib2d = (delegate*<uint, double, double, void>)load("glVertexAttrib2d");
            contextPtr->glVertexAttrib2dv = (delegate*<uint, double*, void>)load("glVertexAttrib2dv");
            contextPtr->glVertexAttrib2f = (delegate*<uint, float, float, void>)load("glVertexAttrib2f");
            contextPtr->glVertexAttrib2fv = (delegate*<uint, float*, void>)load("glVertexAttrib2fv");
            contextPtr->glVertexAttrib2s = (delegate*<uint, short, short, void>)load("glVertexAttrib2s");
            contextPtr->glVertexAttrib2sv = (delegate*<uint, short*, void>)load("glVertexAttrib2sv");
            contextPtr->glVertexAttrib3d = (delegate*<uint, double, double, double, void>)load("glVertexAttrib3d");
            contextPtr->glVertexAttrib3dv = (delegate*<uint, double*, void>)load("glVertexAttrib3dv");
            contextPtr->glVertexAttrib3f = (delegate*<uint, float, float, float, void>)load("glVertexAttrib3f");
            contextPtr->glVertexAttrib3fv = (delegate*<uint, float*, void>)load("glVertexAttrib3fv");
            contextPtr->glVertexAttrib3s = (delegate*<uint, short, short, short, void>)load("glVertexAttrib3s");
            contextPtr->glVertexAttrib3sv = (delegate*<uint, short*, void>)load("glVertexAttrib3sv");
            contextPtr->glVertexAttrib4Nbv = (delegate*<uint, sbyte*, void>)load("glVertexAttrib4Nbv");
            contextPtr->glVertexAttrib4Niv = (delegate*<uint, int*, void>)load("glVertexAttrib4Niv");
            contextPtr->glVertexAttrib4Nsv = (delegate*<uint, short*, void>)load("glVertexAttrib4Nsv");
            contextPtr->glVertexAttrib4Nub = (delegate*<uint, byte, byte, byte, byte, void>)load("glVertexAttrib4Nub");
            contextPtr->glVertexAttrib4Nubv = (delegate*<uint, byte*, void>)load("glVertexAttrib4Nubv");
            contextPtr->glVertexAttrib4Nuiv = (delegate*<uint, uint*, void>)load("glVertexAttrib4Nuiv");
            contextPtr->glVertexAttrib4Nusv = (delegate*<uint, ushort*, void>)load("glVertexAttrib4Nusv");
            contextPtr->glVertexAttrib4bv = (delegate*<uint, sbyte*, void>)load("glVertexAttrib4bv");
            contextPtr->glVertexAttrib4d = (delegate*<uint, double, double, double, double, void>)load("glVertexAttrib4d");
            contextPtr->glVertexAttrib4dv = (delegate*<uint, double*, void>)load("glVertexAttrib4dv");
            contextPtr->glVertexAttrib4f = (delegate*<uint, float, float, float, float, void>)load("glVertexAttrib4f");
            contextPtr->glVertexAttrib4fv = (delegate*<uint, float*, void>)load("glVertexAttrib4fv");
            contextPtr->glVertexAttrib4iv = (delegate*<uint, int*, void>)load("glVertexAttrib4iv");
            contextPtr->glVertexAttrib4s = (delegate*<uint, short, short, short, short, void>)load("glVertexAttrib4s");
            contextPtr->glVertexAttrib4sv = (delegate*<uint, short*, void>)load("glVertexAttrib4sv");
            contextPtr->glVertexAttrib4ubv = (delegate*<uint, byte*, void>)load("glVertexAttrib4ubv");
            contextPtr->glVertexAttrib4uiv = (delegate*<uint, uint*, void>)load("glVertexAttrib4uiv");
            contextPtr->glVertexAttrib4usv = (delegate*<uint, ushort*, void>)load("glVertexAttrib4usv");
            contextPtr->glVertexAttribPointer = (delegate*<uint, int, uint, int, uint, void*, void>)load("glVertexAttribPointer");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 2.0 APIs");
        }

        internal static unsafe void LoadGLcore21(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core21)
            {
                InternalIO.InternalAppendLog("The core version 2.1 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glUniformMatrix2x3fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix2x3fv");
            contextPtr->glUniformMatrix2x4fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix2x4fv");
            contextPtr->glUniformMatrix3x2fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix3x2fv");
            contextPtr->glUniformMatrix3x4fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix3x4fv");
            contextPtr->glUniformMatrix4x2fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix4x2fv");
            contextPtr->glUniformMatrix4x3fv = (delegate*<int, uint, bool, float*, void>)load("glUniformMatrix4x3fv");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 2.1 APIs");
        }

        internal static unsafe void LoadGLcore30(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core30)
            {
                InternalIO.InternalAppendLog("The core version 3.0 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBeginConditionalRender = (delegate*<uint, uint, void>)load("glBeginConditionalRender");
            contextPtr->glBeginTransformFeedback = (delegate*<uint, void>)load("glBeginTransformFeedback");
            contextPtr->glBindBufferBase = (delegate*<uint, uint, uint, void>)load("glBindBufferBase");
            contextPtr->glBindBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load("glBindBufferRange");
            contextPtr->glBindFragDataLocation = (delegate*<uint, uint, byte*, void>)load("glBindFragDataLocation");
            contextPtr->glBindFramebuffer = (delegate*<uint, uint, void>)load("glBindFramebuffer");
            contextPtr->glBindRenderbuffer = (delegate*<uint, uint, void>)load("glBindRenderbuffer");
            contextPtr->glBindVertexArray = (delegate*<uint, void>)load("glBindVertexArray");
            contextPtr->glBlitFramebuffer = (delegate*<int, int, int, int, int, int, int, int, uint, uint, void>)load("glBlitFramebuffer");
            contextPtr->glCheckFramebufferStatus = (delegate*<uint, uint>)load("glCheckFramebufferStatus");
            contextPtr->glClampColor = (delegate*<uint, uint, void>)load("glClampColor");
            contextPtr->glClearBufferfi = (delegate*<uint, int, float, int, void>)load("glClearBufferfi");
            contextPtr->glClearBufferfv = (delegate*<uint, int, float*, void>)load("glClearBufferfv");
            contextPtr->glClearBufferiv = (delegate*<uint, int, int*, void>)load("glClearBufferiv");
            contextPtr->glClearBufferuiv = (delegate*<uint, int, uint*, void>)load("glClearBufferuiv");
            contextPtr->glColorMaski = (delegate*<uint, bool, bool, bool, bool, void>)load("glColorMaski");
            contextPtr->glDeleteFramebuffers = (delegate*<uint, uint*, void>)load("glDeleteFramebuffers");
            contextPtr->glDeleteRenderbuffers = (delegate*<uint, uint*, void>)load("glDeleteRenderbuffers");
            contextPtr->glDeleteVertexArrays = (delegate*<uint, uint*, void>)load("glDeleteVertexArrays");
            contextPtr->glDisablei = (delegate*<uint, uint, void>)load("glDisablei");
            contextPtr->glEnablei = (delegate*<uint, uint, void>)load("glEnablei");
            contextPtr->glEndConditionalRender = (delegate*<void>)load("glEndConditionalRender");
            contextPtr->glEndTransformFeedback = (delegate*<void>)load("glEndTransformFeedback");
            contextPtr->glFlushMappedBufferRange = (delegate*<uint, IntPtr, UIntPtr, void>)load("glFlushMappedBufferRange");
            contextPtr->glFramebufferRenderbuffer = (delegate*<uint, uint, uint, uint, void>)load("glFramebufferRenderbuffer");
            contextPtr->glFramebufferTexture1D = (delegate*<uint, uint, uint, uint, int, void>)load("glFramebufferTexture1D");
            contextPtr->glFramebufferTexture2D = (delegate*<uint, uint, uint, uint, int, void>)load("glFramebufferTexture2D");
            contextPtr->glFramebufferTexture3D = (delegate*<uint, uint, uint, uint, int, int, void>)load("glFramebufferTexture3D");
            contextPtr->glFramebufferTextureLayer = (delegate*<uint, uint, uint, int, int, void>)load("glFramebufferTextureLayer");
            contextPtr->glGenFramebuffers = (delegate*<uint, uint*, void>)load("glGenFramebuffers");
            contextPtr->glGenRenderbuffers = (delegate*<uint, uint*, void>)load("glGenRenderbuffers");
            contextPtr->glGenVertexArrays = (delegate*<uint, uint*, void>)load("glGenVertexArrays");
            contextPtr->glGenerateMipmap = (delegate*<uint, void>)load("glGenerateMipmap");
            contextPtr->glGetBooleani_v = (delegate*<uint, uint, bool*, void>)load("glGetBooleani_v");
            contextPtr->glGetFragDataLocation = (delegate*<uint, byte*, int>)load("glGetFragDataLocation");
            contextPtr->glGetFramebufferAttachmentParameteriv = (delegate*<uint, uint, uint, int*, void>)load("glGetFramebufferAttachmentParameteriv");
            contextPtr->glGetIntegeri_v = (delegate*<uint, uint, int*, void>)load("glGetIntegeri_v");
            contextPtr->glGetRenderbufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetRenderbufferParameteriv");
            contextPtr->glGetStringi = (delegate*<uint, uint, byte*>)load("glGetStringi");
            contextPtr->glGetTexParameterIiv = (delegate*<uint, uint, int*, void>)load("glGetTexParameterIiv");
            contextPtr->glGetTexParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glGetTexParameterIuiv");
            contextPtr->glGetTransformFeedbackVarying = (delegate*<uint, uint, uint, uint*, uint*, uint*, byte*, void>)load("glGetTransformFeedbackVarying");
            contextPtr->glGetUniformuiv = (delegate*<uint, int, uint*, void>)load("glGetUniformuiv");
            contextPtr->glGetVertexAttribIiv = (delegate*<uint, uint, int*, void>)load("glGetVertexAttribIiv");
            contextPtr->glGetVertexAttribIuiv = (delegate*<uint, uint, uint*, void>)load("glGetVertexAttribIuiv");
            contextPtr->glIsEnabledi = (delegate*<uint, uint, bool>)load("glIsEnabledi");
            contextPtr->glIsFramebuffer = (delegate*<uint, bool>)load("glIsFramebuffer");
            contextPtr->glIsRenderbuffer = (delegate*<uint, bool>)load("glIsRenderbuffer");
            contextPtr->glIsVertexArray = (delegate*<uint, bool>)load("glIsVertexArray");
            contextPtr->glMapBufferRange = (delegate*<uint, IntPtr, UIntPtr, uint, void*>)load("glMapBufferRange");
            contextPtr->glRenderbufferStorage = (delegate*<uint, uint, uint, uint, void>)load("glRenderbufferStorage");
            contextPtr->glRenderbufferStorageMultisample = (delegate*<uint, uint, uint, uint, uint, void>)load("glRenderbufferStorageMultisample");
            contextPtr->glTexParameterIiv = (delegate*<uint, uint, int*, void>)load("glTexParameterIiv");
            contextPtr->glTexParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glTexParameterIuiv");
            contextPtr->glTransformFeedbackVaryings = (delegate*<uint, uint, byte*, byte*, uint, void>)load("glTransformFeedbackVaryings");
            contextPtr->glUniform1ui = (delegate*<int, uint, void>)load("glUniform1ui");
            contextPtr->glUniform1uiv = (delegate*<int, uint, uint*, void>)load("glUniform1uiv");
            contextPtr->glUniform2ui = (delegate*<int, uint, uint, void>)load("glUniform2ui");
            contextPtr->glUniform2uiv = (delegate*<int, uint, uint*, void>)load("glUniform2uiv");
            contextPtr->glUniform3ui = (delegate*<int, uint, uint, uint, void>)load("glUniform3ui");
            contextPtr->glUniform3uiv = (delegate*<int, uint, uint*, void>)load("glUniform3uiv");
            contextPtr->glUniform4ui = (delegate*<int, uint, uint, uint, uint, void>)load("glUniform4ui");
            contextPtr->glUniform4uiv = (delegate*<int, uint, uint*, void>)load("glUniform4uiv");
            contextPtr->glVertexAttribI1i = (delegate*<uint, int, void>)load("glVertexAttribI1i");
            contextPtr->glVertexAttribI1iv = (delegate*<uint, int*, void>)load("glVertexAttribI1iv");
            contextPtr->glVertexAttribI1ui = (delegate*<uint, uint, void>)load("glVertexAttribI1ui");
            contextPtr->glVertexAttribI1uiv = (delegate*<uint, uint*, void>)load("glVertexAttribI1uiv");
            contextPtr->glVertexAttribI2i = (delegate*<uint, int, int, void>)load("glVertexAttribI2i");
            contextPtr->glVertexAttribI2iv = (delegate*<uint, int*, void>)load("glVertexAttribI2iv");
            contextPtr->glVertexAttribI2ui = (delegate*<uint, uint, uint, void>)load("glVertexAttribI2ui");
            contextPtr->glVertexAttribI2uiv = (delegate*<uint, uint*, void>)load("glVertexAttribI2uiv");
            contextPtr->glVertexAttribI3i = (delegate*<uint, int, int, int, void>)load("glVertexAttribI3i");
            contextPtr->glVertexAttribI3iv = (delegate*<uint, int*, void>)load("glVertexAttribI3iv");
            contextPtr->glVertexAttribI3ui = (delegate*<uint, uint, uint, uint, void>)load("glVertexAttribI3ui");
            contextPtr->glVertexAttribI3uiv = (delegate*<uint, uint*, void>)load("glVertexAttribI3uiv");
            contextPtr->glVertexAttribI4bv = (delegate*<uint, sbyte*, void>)load("glVertexAttribI4bv");
            contextPtr->glVertexAttribI4i = (delegate*<uint, int, int, int, int, void>)load("glVertexAttribI4i");
            contextPtr->glVertexAttribI4iv = (delegate*<uint, int*, void>)load("glVertexAttribI4iv");
            contextPtr->glVertexAttribI4sv = (delegate*<uint, short*, void>)load("glVertexAttribI4sv");
            contextPtr->glVertexAttribI4ubv = (delegate*<uint, byte*, void>)load("glVertexAttribI4ubv");
            contextPtr->glVertexAttribI4ui = (delegate*<uint, uint, uint, uint, uint, void>)load("glVertexAttribI4ui");
            contextPtr->glVertexAttribI4uiv = (delegate*<uint, uint*, void>)load("glVertexAttribI4uiv");
            contextPtr->glVertexAttribI4usv = (delegate*<uint, ushort*, void>)load("glVertexAttribI4usv");
            contextPtr->glVertexAttribIPointer = (delegate*<uint, int, uint, uint, void*, void>)load("glVertexAttribIPointer");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.0 APIs");
        }

        internal static unsafe void LoadGLcore31(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core31)
            {
                InternalIO.InternalAppendLog("The core version 3.1 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindBufferBase = (delegate*<uint, uint, uint, void>)load("glBindBufferBase");
            contextPtr->glBindBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load("glBindBufferRange");
            contextPtr->glCopyBufferSubData = (delegate*<uint, uint, IntPtr, IntPtr, UIntPtr, void>)load("glCopyBufferSubData");
            contextPtr->glDrawArraysInstanced = (delegate*<uint, int, uint, uint, void>)load("glDrawArraysInstanced");
            contextPtr->glDrawElementsInstanced = (delegate*<uint, uint, uint, void*, uint, void>)load("glDrawElementsInstanced");
            contextPtr->glGetActiveUniformBlockName = (delegate*<uint, uint, uint, uint*, byte*, void>)load("glGetActiveUniformBlockName");
            contextPtr->glGetActiveUniformBlockiv = (delegate*<uint, uint, uint, int*, void>)load("glGetActiveUniformBlockiv");
            contextPtr->glGetActiveUniformName = (delegate*<uint, uint, uint, uint*, byte*, void>)load("glGetActiveUniformName");
            contextPtr->glGetActiveUniformsiv = (delegate*<uint, uint, uint*, uint, int*, void>)load("glGetActiveUniformsiv");
            contextPtr->glGetIntegeri_v = (delegate*<uint, uint, int*, void>)load("glGetIntegeri_v");
            contextPtr->glGetUniformBlockIndex = (delegate*<uint, byte*, uint>)load("glGetUniformBlockIndex");
            contextPtr->glGetUniformIndices = (delegate*<uint, uint, byte*, char**, uint*, void>)load("glGetUniformIndices");
            contextPtr->glPrimitiveRestartIndex = (delegate*<uint, void>)load("glPrimitiveRestartIndex");
            contextPtr->glTexBuffer = (delegate*<uint, uint, uint, void>)load("glTexBuffer");
            contextPtr->glUniformBlockBinding = (delegate*<uint, uint, uint, void>)load("glUniformBlockBinding");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.1 APIs");
        }

        internal static unsafe void LoadGLcore32(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core32)
            {
                InternalIO.InternalAppendLog("The core version 3.2 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glClientWaitSync = (delegate*<GLsync, uint, ulong, uint>)load("glClientWaitSync");
            contextPtr->glDeleteSync = (delegate*<GLsync, void>)load("glDeleteSync");
            contextPtr->glDrawElementsBaseVertex = (delegate*<uint, uint, uint, void*, int, void>)load("glDrawElementsBaseVertex");
            contextPtr->glDrawElementsInstancedBaseVertex = (delegate*<uint, uint, uint, void*, uint, int, void>)load("glDrawElementsInstancedBaseVertex");
            contextPtr->glDrawRangeElementsBaseVertex = (delegate*<uint, uint, uint, uint, uint, void*, int, void>)load("glDrawRangeElementsBaseVertex");
            contextPtr->glFenceSync = (delegate*<uint, uint, GLsync>)load("glFenceSync");
            contextPtr->glFramebufferTexture = (delegate*<uint, uint, uint, int, void>)load("glFramebufferTexture");
            contextPtr->glGetBufferParameteri64v = (delegate*<uint, uint, long*, void>)load("glGetBufferParameteri64v");
            contextPtr->glGetInteger64i_v = (delegate*<uint, uint, long*, void>)load("glGetInteger64i_v");
            contextPtr->glGetInteger64v = (delegate*<uint, long*, void>)load("glGetInteger64v");
            contextPtr->glGetMultisamplefv = (delegate*<uint, uint, float*, void>)load("glGetMultisamplefv");
            contextPtr->glGetSynciv = (delegate*<GLsync, uint, uint, uint*, int*, void>)load("glGetSynciv");
            contextPtr->glIsSync = (delegate*<GLsync, bool>)load("glIsSync");
            contextPtr->glMultiDrawElementsBaseVertex = (delegate*<uint, uint*, uint, void*, void**, uint, int*, void>)load("glMultiDrawElementsBaseVertex");
            contextPtr->glProvokingVertex = (delegate*<uint, void>)load("glProvokingVertex");
            contextPtr->glSampleMaski = (delegate*<uint, uint, void>)load("glSampleMaski");
            contextPtr->glTexImage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load("glTexImage2DMultisample");
            contextPtr->glTexImage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load("glTexImage3DMultisample");
            contextPtr->glWaitSync = (delegate*<GLsync, uint, ulong, void>)load("glWaitSync");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.2 APIs");
        }

        internal static unsafe void LoadGLcore33(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core33)
            {
                InternalIO.InternalAppendLog("The core version 3.3 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindFragDataLocationIndexed = (delegate*<uint, uint, uint, byte*, void>)load("glBindFragDataLocationIndexed");
            contextPtr->glBindSampler = (delegate*<uint, uint, void>)load("glBindSampler");
            contextPtr->glDeleteSamplers = (delegate*<uint, uint*, void>)load("glDeleteSamplers");
            contextPtr->glGenSamplers = (delegate*<uint, uint*, void>)load("glGenSamplers");
            contextPtr->glGetFragDataIndex = (delegate*<uint, byte*, int>)load("glGetFragDataIndex");
            contextPtr->glGetQueryObjecti64v = (delegate*<uint, uint, long*, void>)load("glGetQueryObjecti64v");
            contextPtr->glGetQueryObjectui64v = (delegate*<uint, uint, ulong*, void>)load("glGetQueryObjectui64v");
            contextPtr->glGetSamplerParameterIiv = (delegate*<uint, uint, int*, void>)load("glGetSamplerParameterIiv");
            contextPtr->glGetSamplerParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glGetSamplerParameterIuiv");
            contextPtr->glGetSamplerParameterfv = (delegate*<uint, uint, float*, void>)load("glGetSamplerParameterfv");
            contextPtr->glGetSamplerParameteriv = (delegate*<uint, uint, int*, void>)load("glGetSamplerParameteriv");
            contextPtr->glIsSampler = (delegate*<uint, bool>)load("glIsSampler");
            contextPtr->glQueryCounter = (delegate*<uint, uint, void>)load("glQueryCounter");
            contextPtr->glSamplerParameterIiv = (delegate*<uint, uint, int*, void>)load("glSamplerParameterIiv");
            contextPtr->glSamplerParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glSamplerParameterIuiv");
            contextPtr->glSamplerParameterf = (delegate*<uint, uint, float, void>)load("glSamplerParameterf");
            contextPtr->glSamplerParameterfv = (delegate*<uint, uint, float*, void>)load("glSamplerParameterfv");
            contextPtr->glSamplerParameteri = (delegate*<uint, uint, int, void>)load("glSamplerParameteri");
            contextPtr->glSamplerParameteriv = (delegate*<uint, uint, int*, void>)load("glSamplerParameteriv");
            contextPtr->glVertexAttribDivisor = (delegate*<uint, uint, void>)load("glVertexAttribDivisor");
            contextPtr->glVertexAttribP1ui = (delegate*<uint, uint, bool, uint, void>)load("glVertexAttribP1ui");
            contextPtr->glVertexAttribP1uiv = (delegate*<uint, uint, bool, uint*, void>)load("glVertexAttribP1uiv");
            contextPtr->glVertexAttribP2ui = (delegate*<uint, uint, bool, uint, void>)load("glVertexAttribP2ui");
            contextPtr->glVertexAttribP2uiv = (delegate*<uint, uint, bool, uint*, void>)load("glVertexAttribP2uiv");
            contextPtr->glVertexAttribP3ui = (delegate*<uint, uint, bool, uint, void>)load("glVertexAttribP3ui");
            contextPtr->glVertexAttribP3uiv = (delegate*<uint, uint, bool, uint*, void>)load("glVertexAttribP3uiv");
            contextPtr->glVertexAttribP4ui = (delegate*<uint, uint, bool, uint, void>)load("glVertexAttribP4ui");
            contextPtr->glVertexAttribP4uiv = (delegate*<uint, uint, bool, uint*, void>)load("glVertexAttribP4uiv");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 3.3 APIs");
        }

        internal static unsafe void LoadGLcore40(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core40)
            {
                InternalIO.InternalAppendLog("The core version 4.0 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBeginQueryIndexed = (delegate*<uint, uint, uint, void>)load("glBeginQueryIndexed");
            contextPtr->glBindTransformFeedback = (delegate*<uint, uint, void>)load("glBindTransformFeedback");
            contextPtr->glBlendEquationSeparatei = (delegate*<uint, uint, uint, void>)load("glBlendEquationSeparatei");
            contextPtr->glBlendEquationi = (delegate*<uint, uint, void>)load("glBlendEquationi");
            contextPtr->glBlendFuncSeparatei = (delegate*<uint, uint, uint, uint, uint, void>)load("glBlendFuncSeparatei");
            contextPtr->glBlendFunci = (delegate*<uint, uint, uint, void>)load("glBlendFunci");
            contextPtr->glDeleteTransformFeedbacks = (delegate*<uint, uint*, void>)load("glDeleteTransformFeedbacks");
            contextPtr->glDrawArraysIndirect = (delegate*<uint, void*, void>)load("glDrawArraysIndirect");
            contextPtr->glDrawElementsIndirect = (delegate*<uint, uint, void*, void>)load("glDrawElementsIndirect");
            contextPtr->glDrawTransformFeedback = (delegate*<uint, uint, void>)load("glDrawTransformFeedback");
            contextPtr->glDrawTransformFeedbackStream = (delegate*<uint, uint, uint, void>)load("glDrawTransformFeedbackStream");
            contextPtr->glEndQueryIndexed = (delegate*<uint, uint, void>)load("glEndQueryIndexed");
            contextPtr->glGenTransformFeedbacks = (delegate*<uint, uint*, void>)load("glGenTransformFeedbacks");
            contextPtr->glGetActiveSubroutineName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load("glGetActiveSubroutineName");
            contextPtr->glGetActiveSubroutineUniformName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load("glGetActiveSubroutineUniformName");
            contextPtr->glGetActiveSubroutineUniformiv = (delegate*<uint, uint, uint, uint, int*, void>)load("glGetActiveSubroutineUniformiv");
            contextPtr->glGetProgramStageiv = (delegate*<uint, uint, uint, int*, void>)load("glGetProgramStageiv");
            contextPtr->glGetQueryIndexediv = (delegate*<uint, uint, uint, int*, void>)load("glGetQueryIndexediv");
            contextPtr->glGetSubroutineIndex = (delegate*<uint, uint, byte*, uint>)load("glGetSubroutineIndex");
            contextPtr->glGetSubroutineUniformLocation = (delegate*<uint, uint, byte*, int>)load("glGetSubroutineUniformLocation");
            contextPtr->glGetUniformSubroutineuiv = (delegate*<uint, int, uint*, void>)load("glGetUniformSubroutineuiv");
            contextPtr->glGetUniformdv = (delegate*<uint, int, double*, void>)load("glGetUniformdv");
            contextPtr->glIsTransformFeedback = (delegate*<uint, bool>)load("glIsTransformFeedback");
            contextPtr->glMinSampleShading = (delegate*<float, void>)load("glMinSampleShading");
            contextPtr->glPatchParameterfv = (delegate*<uint, float*, void>)load("glPatchParameterfv");
            contextPtr->glPatchParameteri = (delegate*<uint, int, void>)load("glPatchParameteri");
            contextPtr->glPauseTransformFeedback = (delegate*<void>)load("glPauseTransformFeedback");
            contextPtr->glResumeTransformFeedback = (delegate*<void>)load("glResumeTransformFeedback");
            contextPtr->glUniform1d = (delegate*<int, double, void>)load("glUniform1d");
            contextPtr->glUniform1dv = (delegate*<int, uint, double*, void>)load("glUniform1dv");
            contextPtr->glUniform2d = (delegate*<int, double, double, void>)load("glUniform2d");
            contextPtr->glUniform2dv = (delegate*<int, uint, double*, void>)load("glUniform2dv");
            contextPtr->glUniform3d = (delegate*<int, double, double, double, void>)load("glUniform3d");
            contextPtr->glUniform3dv = (delegate*<int, uint, double*, void>)load("glUniform3dv");
            contextPtr->glUniform4d = (delegate*<int, double, double, double, double, void>)load("glUniform4d");
            contextPtr->glUniform4dv = (delegate*<int, uint, double*, void>)load("glUniform4dv");
            contextPtr->glUniformMatrix2dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix2dv");
            contextPtr->glUniformMatrix2x3dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix2x3dv");
            contextPtr->glUniformMatrix2x4dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix2x4dv");
            contextPtr->glUniformMatrix3dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix3dv");
            contextPtr->glUniformMatrix3x2dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix3x2dv");
            contextPtr->glUniformMatrix3x4dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix3x4dv");
            contextPtr->glUniformMatrix4dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix4dv");
            contextPtr->glUniformMatrix4x2dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix4x2dv");
            contextPtr->glUniformMatrix4x3dv = (delegate*<int, uint, bool, double*, void>)load("glUniformMatrix4x3dv");
            contextPtr->glUniformSubroutinesuiv = (delegate*<uint, uint, uint*, void>)load("glUniformSubroutinesuiv");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.0 APIs");
        }

        internal static unsafe void LoadGLcore41(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core41)
            {
                InternalIO.InternalAppendLog("The core version 4.1 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glActiveShaderProgram = (delegate*<uint, uint, void>)load("glActiveShaderProgram");
            contextPtr->glBindProgramPipeline = (delegate*<uint, void>)load("glBindProgramPipeline");
            contextPtr->glClearDepthf = (delegate*<float, void>)load("glClearDepthf");
            contextPtr->glCreateShaderProgramv = (delegate*<uint, uint, byte*, uint>)load("glCreateShaderProgramv");
            contextPtr->glDeleteProgramPipelines = (delegate*<uint, uint*, void>)load("glDeleteProgramPipelines");
            contextPtr->glDepthRangeArrayv = (delegate*<uint, uint, double*, void>)load("glDepthRangeArrayv");
            contextPtr->glDepthRangeIndexed = (delegate*<uint, double, double, void>)load("glDepthRangeIndexed");
            contextPtr->glDepthRangef = (delegate*<float, float, void>)load("glDepthRangef");
            contextPtr->glGenProgramPipelines = (delegate*<uint, uint*, void>)load("glGenProgramPipelines");
            contextPtr->glGetDoublei_v = (delegate*<uint, uint, double*, void>)load("glGetDoublei_v");
            contextPtr->glGetFloati_v = (delegate*<uint, uint, float*, void>)load("glGetFloati_v");
            contextPtr->glGetProgramBinary = (delegate*<uint, uint, uint*, uint*, void*, void>)load("glGetProgramBinary");
            contextPtr->glGetProgramPipelineInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load("glGetProgramPipelineInfoLog");
            contextPtr->glGetProgramPipelineiv = (delegate*<uint, uint, int*, void>)load("glGetProgramPipelineiv");
            contextPtr->glGetShaderPrecisionFormat = (delegate*<uint, uint, int*, int*, void>)load("glGetShaderPrecisionFormat");
            contextPtr->glGetVertexAttribLdv = (delegate*<uint, uint, double*, void>)load("glGetVertexAttribLdv");
            contextPtr->glIsProgramPipeline = (delegate*<uint, bool>)load("glIsProgramPipeline");
            contextPtr->glProgramBinary = (delegate*<uint, uint, void*, uint, void>)load("glProgramBinary");
            contextPtr->glProgramParameteri = (delegate*<uint, uint, int, void>)load("glProgramParameteri");
            contextPtr->glProgramUniform1d = (delegate*<uint, int, double, void>)load("glProgramUniform1d");
            contextPtr->glProgramUniform1dv = (delegate*<uint, int, uint, double*, void>)load("glProgramUniform1dv");
            contextPtr->glProgramUniform1f = (delegate*<uint, int, float, void>)load("glProgramUniform1f");
            contextPtr->glProgramUniform1fv = (delegate*<uint, int, uint, float*, void>)load("glProgramUniform1fv");
            contextPtr->glProgramUniform1i = (delegate*<uint, int, int, void>)load("glProgramUniform1i");
            contextPtr->glProgramUniform1iv = (delegate*<uint, int, uint, int*, void>)load("glProgramUniform1iv");
            contextPtr->glProgramUniform1ui = (delegate*<uint, int, uint, void>)load("glProgramUniform1ui");
            contextPtr->glProgramUniform1uiv = (delegate*<uint, int, uint, uint*, void>)load("glProgramUniform1uiv");
            contextPtr->glProgramUniform2d = (delegate*<uint, int, double, double, void>)load("glProgramUniform2d");
            contextPtr->glProgramUniform2dv = (delegate*<uint, int, uint, double*, void>)load("glProgramUniform2dv");
            contextPtr->glProgramUniform2f = (delegate*<uint, int, float, float, void>)load("glProgramUniform2f");
            contextPtr->glProgramUniform2fv = (delegate*<uint, int, uint, float*, void>)load("glProgramUniform2fv");
            contextPtr->glProgramUniform2i = (delegate*<uint, int, int, int, void>)load("glProgramUniform2i");
            contextPtr->glProgramUniform2iv = (delegate*<uint, int, uint, int*, void>)load("glProgramUniform2iv");
            contextPtr->glProgramUniform2ui = (delegate*<uint, int, uint, uint, void>)load("glProgramUniform2ui");
            contextPtr->glProgramUniform2uiv = (delegate*<uint, int, uint, uint*, void>)load("glProgramUniform2uiv");
            contextPtr->glProgramUniform3d = (delegate*<uint, int, double, double, double, void>)load("glProgramUniform3d");
            contextPtr->glProgramUniform3dv = (delegate*<uint, int, uint, double*, void>)load("glProgramUniform3dv");
            contextPtr->glProgramUniform3f = (delegate*<uint, int, float, float, float, void>)load("glProgramUniform3f");
            contextPtr->glProgramUniform3fv = (delegate*<uint, int, uint, float*, void>)load("glProgramUniform3fv");
            contextPtr->glProgramUniform3i = (delegate*<uint, int, int, int, int, void>)load("glProgramUniform3i");
            contextPtr->glProgramUniform3iv = (delegate*<uint, int, uint, int*, void>)load("glProgramUniform3iv");
            contextPtr->glProgramUniform3ui = (delegate*<uint, int, uint, uint, uint, void>)load("glProgramUniform3ui");
            contextPtr->glProgramUniform3uiv = (delegate*<uint, int, uint, uint*, void>)load("glProgramUniform3uiv");
            contextPtr->glProgramUniform4d = (delegate*<uint, int, double, double, double, double, void>)load("glProgramUniform4d");
            contextPtr->glProgramUniform4dv = (delegate*<uint, int, uint, double*, void>)load("glProgramUniform4dv");
            contextPtr->glProgramUniform4f = (delegate*<uint, int, float, float, float, float, void>)load("glProgramUniform4f");
            contextPtr->glProgramUniform4fv = (delegate*<uint, int, uint, float*, void>)load("glProgramUniform4fv");
            contextPtr->glProgramUniform4i = (delegate*<uint, int, int, int, int, int, void>)load("glProgramUniform4i");
            contextPtr->glProgramUniform4iv = (delegate*<uint, int, uint, int*, void>)load("glProgramUniform4iv");
            contextPtr->glProgramUniform4ui = (delegate*<uint, int, uint, uint, uint, uint, void>)load("glProgramUniform4ui");
            contextPtr->glProgramUniform4uiv = (delegate*<uint, int, uint, uint*, void>)load("glProgramUniform4uiv");
            contextPtr->glProgramUniformMatrix2dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix2dv");
            contextPtr->glProgramUniformMatrix2fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix2fv");
            contextPtr->glProgramUniformMatrix2x3dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix2x3dv");
            contextPtr->glProgramUniformMatrix2x3fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix2x3fv");
            contextPtr->glProgramUniformMatrix2x4dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix2x4dv");
            contextPtr->glProgramUniformMatrix2x4fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix2x4fv");
            contextPtr->glProgramUniformMatrix3dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix3dv");
            contextPtr->glProgramUniformMatrix3fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix3fv");
            contextPtr->glProgramUniformMatrix3x2dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix3x2dv");
            contextPtr->glProgramUniformMatrix3x2fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix3x2fv");
            contextPtr->glProgramUniformMatrix3x4dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix3x4dv");
            contextPtr->glProgramUniformMatrix3x4fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix3x4fv");
            contextPtr->glProgramUniformMatrix4dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix4dv");
            contextPtr->glProgramUniformMatrix4fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix4fv");
            contextPtr->glProgramUniformMatrix4x2dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix4x2dv");
            contextPtr->glProgramUniformMatrix4x2fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix4x2fv");
            contextPtr->glProgramUniformMatrix4x3dv = (delegate*<uint, int, uint, bool, double*, void>)load("glProgramUniformMatrix4x3dv");
            contextPtr->glProgramUniformMatrix4x3fv = (delegate*<uint, int, uint, bool, float*, void>)load("glProgramUniformMatrix4x3fv");
            contextPtr->glReleaseShaderCompiler = (delegate*<void>)load("glReleaseShaderCompiler");
            contextPtr->glScissorArrayv = (delegate*<uint, uint, int*, void>)load("glScissorArrayv");
            contextPtr->glScissorIndexed = (delegate*<uint, int, int, uint, uint, void>)load("glScissorIndexed");
            contextPtr->glScissorIndexedv = (delegate*<uint, int*, void>)load("glScissorIndexedv");
            contextPtr->glShaderBinary = (delegate*<uint, uint*, uint, void*, uint, void>)load("glShaderBinary");
            contextPtr->glUseProgramStages = (delegate*<uint, uint, uint, void>)load("glUseProgramStages");
            contextPtr->glValidateProgramPipeline = (delegate*<uint, void>)load("glValidateProgramPipeline");
            contextPtr->glVertexAttribL1d = (delegate*<uint, double, void>)load("glVertexAttribL1d");
            contextPtr->glVertexAttribL1dv = (delegate*<uint, double*, void>)load("glVertexAttribL1dv");
            contextPtr->glVertexAttribL2d = (delegate*<uint, double, double, void>)load("glVertexAttribL2d");
            contextPtr->glVertexAttribL2dv = (delegate*<uint, double*, void>)load("glVertexAttribL2dv");
            contextPtr->glVertexAttribL3d = (delegate*<uint, double, double, double, void>)load("glVertexAttribL3d");
            contextPtr->glVertexAttribL3dv = (delegate*<uint, double*, void>)load("glVertexAttribL3dv");
            contextPtr->glVertexAttribL4d = (delegate*<uint, double, double, double, double, void>)load("glVertexAttribL4d");
            contextPtr->glVertexAttribL4dv = (delegate*<uint, double*, void>)load("glVertexAttribL4dv");
            contextPtr->glVertexAttribLPointer = (delegate*<uint, int, uint, uint, void*, void>)load("glVertexAttribLPointer");
            contextPtr->glViewportArrayv = (delegate*<uint, uint, float*, void>)load("glViewportArrayv");
            contextPtr->glViewportIndexedf = (delegate*<uint, float, float, float, float, void>)load("glViewportIndexedf");
            contextPtr->glViewportIndexedfv = (delegate*<uint, float*, void>)load("glViewportIndexedfv");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.1 APIs");
        }

        internal static unsafe void LoadGLcore42(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core42)
            {
                InternalIO.InternalAppendLog("The core version 4.2 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindImageTexture = (delegate*<uint, uint, int, bool, int, uint, uint, void>)load("glBindImageTexture");
            contextPtr->glDrawArraysInstancedBaseInstance = (delegate*<uint, int, uint, uint, uint, void>)load("glDrawArraysInstancedBaseInstance");
            contextPtr->glDrawElementsInstancedBaseInstance = (delegate*<uint, uint, uint, void*, uint, uint, void>)load("glDrawElementsInstancedBaseInstance");
            contextPtr->glDrawElementsInstancedBaseVertexBaseInstance = (delegate*<uint, uint, uint, void*, uint, int, uint, void>)load("glDrawElementsInstancedBaseVertexBaseInstance");
            contextPtr->glDrawTransformFeedbackInstanced = (delegate*<uint, uint, uint, void>)load("glDrawTransformFeedbackInstanced");
            contextPtr->glDrawTransformFeedbackStreamInstanced = (delegate*<uint, uint, uint, uint, void>)load("glDrawTransformFeedbackStreamInstanced");
            contextPtr->glGetActiveAtomicCounterBufferiv = (delegate*<uint, uint, uint, int*, void>)load("glGetActiveAtomicCounterBufferiv");
            contextPtr->glGetInternalformativ = (delegate*<uint, uint, uint, uint, int*, void>)load("glGetInternalformativ");
            contextPtr->glMemoryBarrier = (delegate*<uint, void>)load("glMemoryBarrier");
            contextPtr->glTexStorage1D = (delegate*<uint, uint, uint, uint, void>)load("glTexStorage1D");
            contextPtr->glTexStorage2D = (delegate*<uint, uint, uint, uint, uint, void>)load("glTexStorage2D");
            contextPtr->glTexStorage3D = (delegate*<uint, uint, uint, uint, uint, uint, void>)load("glTexStorage3D");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.2 APIs");
        }

        internal static unsafe void LoadGLcore43(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core43)
            {
                InternalIO.InternalAppendLog("The core version 4.3 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindVertexBuffer = (delegate*<uint, uint, IntPtr, uint, void>)load("glBindVertexBuffer");
            contextPtr->glClearBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load("glClearBufferData");
            contextPtr->glClearBufferSubData = (delegate*<uint, uint, IntPtr, UIntPtr, uint, uint, void*, void>)load("glClearBufferSubData");
            contextPtr->glCopyImageSubData = (delegate*<uint, uint, int, int, int, int, uint, uint, int, int, int, int, uint, uint, uint, void>)load("glCopyImageSubData");
            contextPtr->glDebugMessageCallback = (delegate*<GLDEBUGPROC, void*, void>)load("glDebugMessageCallback");
            contextPtr->glDebugMessageControl = (delegate*<uint, uint, uint, uint, uint*, bool, void>)load("glDebugMessageControl");
            contextPtr->glDebugMessageInsert = (delegate*<uint, uint, uint, uint, uint, byte*, void>)load("glDebugMessageInsert");
            contextPtr->glDispatchCompute = (delegate*<uint, uint, uint, void>)load("glDispatchCompute");
            contextPtr->glDispatchComputeIndirect = (delegate*<IntPtr, void>)load("glDispatchComputeIndirect");
            contextPtr->glFramebufferParameteri = (delegate*<uint, uint, int, void>)load("glFramebufferParameteri");
            contextPtr->glGetDebugMessageLog = (delegate*<uint, uint, uint*, uint*, uint*, uint*, uint*, byte*, uint>)load("glGetDebugMessageLog");
            contextPtr->glGetFramebufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetFramebufferParameteriv");
            contextPtr->glGetInternalformati64v = (delegate*<uint, uint, uint, uint, long*, void>)load("glGetInternalformati64v");
            contextPtr->glGetObjectLabel = (delegate*<uint, uint, uint, uint*, byte*, void>)load("glGetObjectLabel");
            contextPtr->glGetObjectPtrLabel = (delegate*<void*, uint, uint*, byte*, void>)load("glGetObjectPtrLabel");
            contextPtr->glGetPointerv = (delegate*<uint, void**, void>)load("glGetPointerv");
            contextPtr->glGetProgramInterfaceiv = (delegate*<uint, uint, uint, int*, void>)load("glGetProgramInterfaceiv");
            contextPtr->glGetProgramResourceIndex = (delegate*<uint, uint, byte*, uint>)load("glGetProgramResourceIndex");
            contextPtr->glGetProgramResourceLocation = (delegate*<uint, uint, byte*, int>)load("glGetProgramResourceLocation");
            contextPtr->glGetProgramResourceLocationIndex = (delegate*<uint, uint, byte*, int>)load("glGetProgramResourceLocationIndex");
            contextPtr->glGetProgramResourceName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load("glGetProgramResourceName");
            contextPtr->glGetProgramResourceiv = (delegate*<uint, uint, uint, uint, uint*, uint, uint*, int*, void>)load("glGetProgramResourceiv");
            contextPtr->glInvalidateBufferData = (delegate*<uint, void>)load("glInvalidateBufferData");
            contextPtr->glInvalidateBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void>)load("glInvalidateBufferSubData");
            contextPtr->glInvalidateFramebuffer = (delegate*<uint, uint, uint*, void>)load("glInvalidateFramebuffer");
            contextPtr->glInvalidateSubFramebuffer = (delegate*<uint, uint, uint*, int, int, uint, uint, void>)load("glInvalidateSubFramebuffer");
            contextPtr->glInvalidateTexImage = (delegate*<uint, int, void>)load("glInvalidateTexImage");
            contextPtr->glInvalidateTexSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, void>)load("glInvalidateTexSubImage");
            contextPtr->glMultiDrawArraysIndirect = (delegate*<uint, void*, uint, uint, void>)load("glMultiDrawArraysIndirect");
            contextPtr->glMultiDrawElementsIndirect = (delegate*<uint, uint, void*, uint, uint, void>)load("glMultiDrawElementsIndirect");
            contextPtr->glObjectLabel = (delegate*<uint, uint, uint, byte*, void>)load("glObjectLabel");
            contextPtr->glObjectPtrLabel = (delegate*<void*, uint, byte*, void>)load("glObjectPtrLabel");
            contextPtr->glPopDebugGroup = (delegate*<void>)load("glPopDebugGroup");
            contextPtr->glPushDebugGroup = (delegate*<uint, uint, uint, byte*, void>)load("glPushDebugGroup");
            contextPtr->glShaderStorageBlockBinding = (delegate*<uint, uint, uint, void>)load("glShaderStorageBlockBinding");
            contextPtr->glTexBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load("glTexBufferRange");
            contextPtr->glTexStorage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load("glTexStorage2DMultisample");
            contextPtr->glTexStorage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load("glTexStorage3DMultisample");
            contextPtr->glTextureView = (delegate*<uint, uint, uint, uint, uint, uint, uint, uint, void>)load("glTextureView");
            contextPtr->glVertexAttribBinding = (delegate*<uint, uint, void>)load("glVertexAttribBinding");
            contextPtr->glVertexAttribFormat = (delegate*<uint, int, uint, bool, uint, void>)load("glVertexAttribFormat");
            contextPtr->glVertexAttribIFormat = (delegate*<uint, int, uint, uint, void>)load("glVertexAttribIFormat");
            contextPtr->glClearBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load("glClearBufferData");
            contextPtr->glVertexBindingDivisor = (delegate*<uint, uint, void>)load("glVertexBindingDivisor");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.3 APIs");
        }

        internal static unsafe void LoadGLcore44(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core44)
            {
                InternalIO.InternalAppendLog("The core version 4.4 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindBuffersBase = (delegate*<uint, uint, uint, uint*, void>)load("glBindBuffersBase");
            contextPtr->glBindBuffersRange = (delegate*<uint, uint, uint, uint*, IntPtr*, UIntPtr*, void>)load("glBindBuffersRange");
            contextPtr->glBindImageTextures = (delegate*<uint, uint, uint*, void>)load("glBindImageTextures");
            contextPtr->glBindSamplers = (delegate*<uint, uint, uint*, void>)load("glBindSamplers");
            contextPtr->glBindTextures = (delegate*<uint, uint, uint*, void>)load("glBindTextures");
            contextPtr->glBindVertexBuffers = (delegate*<uint, uint, uint*, IntPtr*, uint*, void>)load("glBindVertexBuffers");
            contextPtr->glBufferStorage = (delegate*<uint, UIntPtr, void*, uint, void>)load("glBufferStorage");
            contextPtr->glClearTexImage = (delegate*<uint, int, uint, uint, void*, void>)load("glClearTexImage");
            contextPtr->glClearTexSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load("glClearTexSubImage");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.4 APIs");
        }

        internal static unsafe void LoadGLcore45(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core45)
            {
                InternalIO.InternalAppendLog("The core version 4.5 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glBindTextureUnit = (delegate*<uint, uint, void>)load("glBindTextureUnit");
            contextPtr->glBlitNamedFramebuffer = (delegate*<uint, uint, int, int, int, int, int, int, int, int, uint, uint, void>)load("glBlitNamedFramebuffer");
            contextPtr->glCheckNamedFramebufferStatus = (delegate*<uint, uint, uint>)load("glCheckNamedFramebufferStatus");
            contextPtr->glClearNamedBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load("glClearNamedBufferData");
            contextPtr->glClearNamedBufferSubData = (delegate*<uint, uint, IntPtr, UIntPtr, uint, uint, void*, void>)load("glClearNamedBufferSubData");
            contextPtr->glClearNamedFramebufferfi = (delegate*<uint, uint, int, float, int, void>)load("glClearNamedFramebufferfi");
            contextPtr->glClearNamedFramebufferfv = (delegate*<uint, uint, int, float*, void>)load("glClearNamedFramebufferfv");
            contextPtr->glClearNamedFramebufferiv = (delegate*<uint, uint, int, int*, void>)load("glClearNamedFramebufferiv");
            contextPtr->glClearNamedFramebufferuiv = (delegate*<uint, uint, int, uint*, void>)load("glClearNamedFramebufferuiv");
            contextPtr->glClipControl = (delegate*<uint, uint, void>)load("glClipControl");
            contextPtr->glCompressedTextureSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load("glCompressedTextureSubImage1D");
            contextPtr->glCompressedTextureSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load("glCompressedTextureSubImage2D");
            contextPtr->glCompressedTextureSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load("glCompressedTextureSubImage3D");
            contextPtr->glCopyNamedBufferSubData = (delegate*<uint, uint, IntPtr, IntPtr, UIntPtr, void>)load("glCopyNamedBufferSubData");
            contextPtr->glCopyTextureSubImage1D = (delegate*<uint, int, int, int, int, uint, void>)load("glCopyTextureSubImage1D");
            contextPtr->glCopyTextureSubImage2D = (delegate*<uint, int, int, int, int, int, uint, uint, void>)load("glCopyTextureSubImage2D");
            contextPtr->glCopyTextureSubImage3D = (delegate*<uint, int, int, int, int, int, int, uint, uint, void>)load("glCopyTextureSubImage3D");
            contextPtr->glCreateBuffers = (delegate*<uint, uint*, void>)load("glCreateBuffers");
            contextPtr->glCreateFramebuffers = (delegate*<uint, uint*, void>)load("glCreateFramebuffers");
            contextPtr->glCreateProgramPipelines = (delegate*<uint, uint*, void>)load("glCreateProgramPipelines");
            contextPtr->glCreateQueries = (delegate*<uint, uint, uint*, void>)load("glCreateQueries");
            contextPtr->glCreateRenderbuffers = (delegate*<uint, uint*, void>)load("glCreateRenderbuffers");
            contextPtr->glCreateSamplers = (delegate*<uint, uint*, void>)load("glCreateSamplers");
            contextPtr->glCreateTextures = (delegate*<uint, uint, uint*, void>)load("glCreateTextures");
            contextPtr->glCreateTransformFeedbacks = (delegate*<uint, uint*, void>)load("glCreateTransformFeedbacks");
            contextPtr->glCreateVertexArrays = (delegate*<uint, uint*, void>)load("glCreateVertexArrays");
            contextPtr->glDisableVertexArrayAttrib = (delegate*<uint, uint, void>)load("glDisableVertexArrayAttrib");
            contextPtr->glEnableVertexArrayAttrib = (delegate*<uint, uint, void>)load("glEnableVertexArrayAttrib");
            contextPtr->glFlushMappedNamedBufferRange = (delegate*<uint, IntPtr, UIntPtr, void>)load("glFlushMappedNamedBufferRange");
            contextPtr->glGenerateTextureMipmap = (delegate*<uint, void>)load("glGenerateTextureMipmap");
            contextPtr->glGetCompressedTextureImage = (delegate*<uint, int, uint, void*, void>)load("glGetCompressedTextureImage");
            contextPtr->glGetCompressedTextureSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, void*, void>)load("glGetCompressedTextureSubImage");
            contextPtr->glGetGraphicsResetStatus = (delegate*<uint>)load("glGetGraphicsResetStatus");
            contextPtr->glGetNamedBufferParameteri64v = (delegate*<uint, uint, long*, void>)load("glGetNamedBufferParameteri64v");
            contextPtr->glGetNamedBufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetNamedBufferParameteriv");
            contextPtr->glGetNamedBufferPointerv = (delegate*<uint, uint, void**, void>)load("glGetNamedBufferPointerv");
            contextPtr->glGetNamedBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load("glGetNamedBufferSubData");
            contextPtr->glGetNamedFramebufferAttachmentParameteriv = (delegate*<uint, uint, uint, int*, void>)load("glGetNamedFramebufferAttachmentParameteriv");
            contextPtr->glGetNamedFramebufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetNamedFramebufferParameteriv");
            contextPtr->glGetNamedRenderbufferParameteriv = (delegate*<uint, uint, int*, void>)load("glGetNamedRenderbufferParameteriv");
            contextPtr->glGetQueryBufferObjecti64v = (delegate*<uint, uint, uint, IntPtr, void>)load("glGetQueryBufferObjecti64v");
            contextPtr->glGetQueryBufferObjectiv = (delegate*<uint, uint, uint, IntPtr, void>)load("glGetQueryBufferObjectiv");
            contextPtr->glGetQueryBufferObjectui64v = (delegate*<uint, uint, uint, IntPtr, void>)load("glGetQueryBufferObjectui64v");
            contextPtr->glGetQueryBufferObjectuiv = (delegate*<uint, uint, uint, IntPtr, void>)load("glGetQueryBufferObjectuiv");
            contextPtr->glGetTextureImage = (delegate*<uint, int, uint, uint, uint, void*, void>)load("glGetTextureImage");
            contextPtr->glGetTextureLevelParameterfv = (delegate*<uint, int, uint, float*, void>)load("glGetTextureLevelParameterfv");
            contextPtr->glGetTextureLevelParameteriv = (delegate*<uint, int, uint, int*, void>)load("glGetTextureLevelParameteriv");
            contextPtr->glGetTextureParameterIiv = (delegate*<uint, uint, int*, void>)load("glGetTextureParameterIiv");
            contextPtr->glGetTextureParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glGetTextureParameterIuiv");
            contextPtr->glGetTextureParameterfv = (delegate*<uint, uint, float*, void>)load("glGetTextureParameterfv");
            contextPtr->glGetTextureParameteriv = (delegate*<uint, uint, int*, void>)load("glGetTextureParameteriv");
            contextPtr->glGetTextureSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, uint, void*, void>)load("glGetTextureSubImage");
            contextPtr->glGetTransformFeedbacki64_v = (delegate*<uint, uint, uint, long*, void>)load("glGetTransformFeedbacki64_v");
            contextPtr->glGetTransformFeedbacki_v = (delegate*<uint, uint, uint, int*, void>)load("glGetTransformFeedbacki_v");
            contextPtr->glGetTransformFeedbackiv = (delegate*<uint, uint, int*, void>)load("glGetTransformFeedbackiv");
            contextPtr->glGetVertexArrayIndexed64iv = (delegate*<uint, uint, uint, long*, void>)load("glGetVertexArrayIndexed64iv");
            contextPtr->glGetVertexArrayIndexediv = (delegate*<uint, uint, uint, int*, void>)load("glGetVertexArrayIndexediv");
            contextPtr->glGetVertexArrayiv = (delegate*<uint, uint, int*, void>)load("glGetVertexArrayiv");
            contextPtr->glGetnCompressedTexImage = (delegate*<uint, int, uint, void*, void>)load("glGetnCompressedTexImage");
            contextPtr->glGetnTexImage = (delegate*<uint, int, uint, uint, uint, void*, void>)load("glGetnTexImage");
            contextPtr->glGetnUniformdv = (delegate*<uint, int, uint, double*, void>)load("glGetnUniformdv");
            contextPtr->glGetnUniformfv = (delegate*<uint, int, uint, float*, void>)load("glGetnUniformfv");
            contextPtr->glGetnUniformiv = (delegate*<uint, int, uint, int*, void>)load("glGetnUniformiv");
            contextPtr->glGetnUniformuiv = (delegate*<uint, int, uint, uint*, void>)load("glGetnUniformuiv");
            contextPtr->glInvalidateNamedFramebufferData = (delegate*<uint, uint, uint*, void>)load("glInvalidateNamedFramebufferData");
            contextPtr->glInvalidateNamedFramebufferSubData = (delegate*<uint, uint, uint*, int, int, uint, uint, void>)load("glInvalidateNamedFramebufferSubData");
            contextPtr->glMapNamedBuffer = (delegate*<uint, uint, void*>)load("glMapNamedBuffer");
            contextPtr->glMapNamedBufferRange = (delegate*<uint, IntPtr, UIntPtr, uint, void*>)load("glMapNamedBufferRange");
            contextPtr->glMemoryBarrierByRegion = (delegate*<uint, void>)load("glMemoryBarrierByRegion");
            contextPtr->glNamedBufferData = (delegate*<uint, UIntPtr, void*, uint, void>)load("glNamedBufferData");
            contextPtr->glNamedBufferStorage = (delegate*<uint, UIntPtr, void*, uint, void>)load("glNamedBufferStorage");
            contextPtr->glNamedBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load("glNamedBufferSubData");
            contextPtr->glNamedFramebufferDrawBuffer = (delegate*<uint, uint, void>)load("glNamedFramebufferDrawBuffer");
            contextPtr->glNamedFramebufferDrawBuffers = (delegate*<uint, uint, uint*, void>)load("glNamedFramebufferDrawBuffers");
            contextPtr->glNamedFramebufferParameteri = (delegate*<uint, uint, int, void>)load("glNamedFramebufferParameteri");
            contextPtr->glNamedFramebufferReadBuffer = (delegate*<uint, uint, void>)load("glNamedFramebufferReadBuffer");
            contextPtr->glNamedFramebufferRenderbuffer = (delegate*<uint, uint, uint, uint, void>)load("glNamedFramebufferRenderbuffer");
            contextPtr->glNamedFramebufferTexture = (delegate*<uint, uint, uint, int, void>)load("glNamedFramebufferTexture");
            contextPtr->glNamedFramebufferTextureLayer = (delegate*<uint, uint, uint, int, int, void>)load("glNamedFramebufferTextureLayer");
            contextPtr->glNamedRenderbufferStorage = (delegate*<uint, uint, uint, uint, void>)load("glNamedRenderbufferStorage");
            contextPtr->glNamedRenderbufferStorageMultisample = (delegate*<uint, uint, uint, uint, uint, void>)load("glNamedRenderbufferStorageMultisample");
            contextPtr->glReadnPixels = (delegate*<int, int, uint, uint, uint, uint, uint, void*, void>)load("glReadnPixels");
            contextPtr->glTextureBarrier = (delegate*<void>)load("glTextureBarrier");
            contextPtr->glTextureBuffer = (delegate*<uint, uint, uint, void>)load("glTextureBuffer");
            contextPtr->glTextureBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load("glTextureBufferRange");
            contextPtr->glTextureParameterIiv = (delegate*<uint, uint, int*, void>)load("glTextureParameterIiv");
            contextPtr->glTextureParameterIuiv = (delegate*<uint, uint, uint*, void>)load("glTextureParameterIuiv");
            contextPtr->glTextureParameterf = (delegate*<uint, uint, float, void>)load("glTextureParameterf");
            contextPtr->glTextureParameterfv = (delegate*<uint, uint, float*, void>)load("glTextureParameterfv");
            contextPtr->glTextureParameteri = (delegate*<uint, uint, int, void>)load("glTextureParameteri");
            contextPtr->glTextureParameteriv = (delegate*<uint, uint, int*, void>)load("glTextureParameteriv");
            contextPtr->glTextureStorage1D = (delegate*<uint, uint, uint, uint, void>)load("glTextureStorage1D");
            contextPtr->glTextureStorage2D = (delegate*<uint, uint, uint, uint, uint, void>)load("glTextureStorage2D");
            contextPtr->glTextureStorage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load("glTextureStorage2DMultisample");
            contextPtr->glTextureStorage3D = (delegate*<uint, uint, uint, uint, uint, uint, void>)load("glTextureStorage3D");
            contextPtr->glTextureStorage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load("glTextureStorage3DMultisample");
            contextPtr->glTextureSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load("glTextureSubImage1D");
            contextPtr->glTextureSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load("glTextureSubImage2D");
            contextPtr->glTextureSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load("glTextureSubImage3D");
            contextPtr->glTransformFeedbackBufferBase = (delegate*<uint, uint, uint, void>)load("glTransformFeedbackBufferBase");
            contextPtr->glTransformFeedbackBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load("glTransformFeedbackBufferRange");
            contextPtr->glUnmapNamedBuffer = (delegate*<uint, bool>)load("glUnmapNamedBuffer");
            contextPtr->glVertexArrayAttribBinding = (delegate*<uint, uint, uint, void>)load("glVertexArrayAttribBinding");
            contextPtr->glVertexArrayAttribFormat = (delegate*<uint, uint, int, uint, bool, uint, void>)load("glVertexArrayAttribFormat");
            contextPtr->glVertexArrayAttribIFormat = (delegate*<uint, uint, int, uint, uint, void>)load("glVertexArrayAttribIFormat");
            contextPtr->glVertexArrayAttribLFormat = (delegate*<uint, uint, int, uint, uint, void>)load("glVertexArrayAttribLFormat");
            contextPtr->glVertexArrayBindingDivisor = (delegate*<uint, uint, uint, void>)load("glVertexArrayBindingDivisor");
            contextPtr->glVertexArrayElementBuffer = (delegate*<uint, uint, void>)load("glVertexArrayElementBuffer");
            contextPtr->glVertexArrayVertexBuffer = (delegate*<uint, uint, uint, IntPtr, uint, void>)load("glVertexArrayVertexBuffer");
            contextPtr->glVertexArrayVertexBuffers = (delegate*<uint, uint, uint, uint*, IntPtr*, uint*, void>)load("glVertexArrayVertexBuffers");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.5 APIs");
        }

        internal static unsafe void LoadGLcore46(GLcontext* contextPtr, GLfuncLoader load)
        {
            if (!core46)
            {
                InternalIO.InternalAppendLog("The core version 4.6 is not supported.");
                throw new Exception("Final Version");
            }

            #region load

            contextPtr->glMultiDrawArraysIndirectCount = (delegate*<uint, void*, IntPtr, uint, uint, void>)load("glMultiDrawArraysIndirectCount");
            contextPtr->glMultiDrawElementsIndirectCount = (delegate*<uint, uint, void*, IntPtr, uint, uint, void>)load("glMultiDrawElementsIndirectCount");
            contextPtr->glPolygonOffsetClamp = (delegate*<float, float, float, void>)load("glPolygonOffsetClamp");
            contextPtr->glSpecializeShader = (delegate*<uint, byte*, uint, uint*, uint*, void>)load("glSpecializeShader");

            #endregion load

            InternalIO.InternalAppendLog("Successfully load all OpenGL 4.6 APIs");
        }

    }
}