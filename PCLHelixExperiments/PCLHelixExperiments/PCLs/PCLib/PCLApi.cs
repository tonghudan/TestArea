using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PCLHelixExperiments.PCLs
{
    class PCLApi
    {
        [DllImport("viewer.dll")]
        public static extern IntPtr getpointsapi(string cloudfilePath, out int size, int type = 0);

        [DllImport("downsampling.dll")]
        public static extern int downsamplingapi(string cloud_file, string out_downsampled_file, string out_outlierremoval_file, string out_border_file, int meanK = 50, float std_dev_mul = 2.0f, float std_mul = 0.5f, float radius = 10, int min_pts = 200, int min_point_limit = 50, float min_std = 5000, int max_keep_size = 10000);

        [DllImport("downsampling.dll")]
        public static extern int downsamplingxyzapi(string cloud_file, string out_downsampled_file, string out_outlierremoval_file, string out_border_file, float minx, float maxx, float miny, float maxy, float minz, float maxz, int meanK = 50, float std_dev_mul = 2.0f, float std_mul = 0.5f, float radius = 10, int min_pts = 200, int min_point_limit = 50, float min_std = 5000, int max_keep_size = 10000, int min_cloud_size = 5000);

        [DllImport("downsampling.dll")]
        public static extern int downsamplingxyz_sor_voxelapi(string cloud_file, string out_downsampled_file, string out_outlierremoval_file, string out_border_file, float minx, float maxx, float miny, float maxy, float minz, float maxz, int meanK = 50, float std_dev_mul = 2.0f, float std_mul = 0.5f, float radius = 10, int min_pts = 200, int min_point_limit = 50, float min_std = 5000, int max_keep_size = 10000, int min_cloud_size = 5000, float voxel_grid_size = 10);

        [DllImport("downsampling.dll")]
        public static extern bool extractKeypointsapi(string input_cloud_file, string output_cloud_file, float gridsize = 10, int meanK = 50, float std_dev_mul = 1.5f, float std_mul = 0.5f, float minx = -99999, float maxx = 99999, float miny = -99999, float maxy = 99999, float minz = -99999, float maxz = 99999, bool debug = false);

        [DllImport("downsampling.dll")]
        public static extern void projectionapi(string cloud_file, string out_downsampled_file, string out_outlierremoval_file, string out_border_file, int meanK = 50, float std_dev_mul = 1.0f, float radius = 10, int min_pts = 200, int min_point_limit = 50, float min_std = 5000, int max_keep_size = 10000);

        [DllImport("extractborder.dll")]
        public static extern void extractborderapi(string cloud_file, string downsampled_file, string out_file, string load_file = null, float pointRadius = 30, int pointLimit = 4, float coeffRadius = 0.9f, float lineRadius = 9.0f, float coeffLimit = 100,
            float line_dist_td = 9.0f, float line_size_td = 0.8f, float line_len_td = 100, float final_line_dist_td = 4.0f, float intersection_dist_td = 500.0f);

        [DllImport("viewer.dll")]
        public static extern void clipplaneapi(string cloud_file, float a, float b, float c, float d);

        [DllImport("viewer.dll")]
        public static extern void clipdepthapi(string cloud_file, float low, float high);

        [DllImport("modelmatch.dll")]
        public static extern int modelmatchoneapi(string cloud_tr_file, string model_file, StringBuilder sb, int iterations = 30);

        [DllImport("modelmatch.dll")]
        public static extern int modelmatchapi(string cloud_tr_file, string model_dir, StringBuilder sb, int iterations = 30);

        [DllImport("modelmatch.dll")]
        public static extern void converttopcdapi(string config_dir, string pcd_dir);

        [DllImport("modelmatch.dll")]
        public static extern void calibratestandardboard_modelmatchapi(string file_name, string board_file_name, double transE = 1e-8, double euclideanFE = 1, int iterations = 100, int planar_threshold = 15);

        [DllImport("ApproxMVBBExample-MVBB.dll")]
        public static extern bool legalityCheckapi(string cloud_model_file, string cloud_match_file, float volumn_diff_ratio_threshold = 0.3f, float cloudsize_diff_ratio_threshold = 0.5f);

        [DllImport("ApproxMVBBExample-MVBB.dll")]
        public static extern void boundingBoxViewapi(string cloud_file);

        [DllImport("viewer.dll")]
        public static extern void ply2pcdapi(string plyfile, string pcdfile);

        [DllImport("viewer.dll")]
        public static extern void pcd2plyapi(string pcdfile, string plyfile);

        [DllImport("viewer.dll")]
        public static extern void pcd2ply_xyzinormalapi(string pcdfile, string plyfile);

        [DllImport("viewer.dll")]
        public static extern void pcd2pcdtxtapi(string pcdfile, string pcdtxtfile);

        [DllImport("viewer.dll")]
        public static extern void clipboxapi(string cloud_in, string cloud_out, float minx, float maxx, float miny, float maxy, float minz, float maxz, float rx, float ry, float rz);

        [DllImport("viewer.dll")]
        public static extern void pcdmultiapi(string inputcloud, string outputcloud, int times);

        [DllImport("pretreatment.dll")]
        public static extern void transformtocenterzeroapi(string cloud_in, string cloud_out);

        [DllImport("pretreatment.dll")]
        public static extern int calcTransformapi(double[] p1, double[] p2, double[] p3, double[] pw1, double[] pw2, double[] pw3, StringBuilder sb);

        [DllImport("pretreatment.dll")]
        public static extern void calibratestandardboardapi(string file_name, double transE = 1e-8, double euclideanFE = 1, int planar_threshold = 15, int kernel_size = 10, int meanK = 50, float std_dev_mul = 1, int min_point_limit = 50, int max_keep_size = 10000, int iteration = 50, int line_width = 10);

        [DllImport("pretreatment.dll")]
        public static extern void calibratestandardboard_newapi(string file_name, double transE = 1e-8, double euclideanFE = 1, int planar_threshold = 15, int kernel_size = 10, int meanK = 50, float std_dev_mul = 1, int min_point_limit = 50, int max_keep_size = 10000, int iteration = 50, int line_width = 10);

        [DllImport("pretreatment.dll")]
        public static extern bool greedyTriangulationapi(string input_file, string output_file, int k_search = 20, double search_radius = 25, double mu = 2.5, int maximum_nearest_neighbors = 100);

        [DllImport("pretreatment.dll")]
        public static extern void generateStandardBoardapi(string input_file, string output_file, float z_cut_threshold);

        [DllImport("pretreatment.dll")]
        public static extern void calibratestandardboard_bestapi(string file_name, string board_file_name, double transE = 1e-8, double euclideanFE = 1, int iteration = 50, int planar_threshold = 15);

        [DllImport("pretreatment.dll")]
        public static extern bool transformPointCloudapi(string input_cloud_file, string input_matrix_file, string output_file);

        [DllImport("pretreatment.dll")]
        public static extern void cloudsmoothapi(string input_file, string output_file);

        [DllImport("pretreatment.dll")]
        public static extern void cutTranslateCloudapi(string input_cloud_file, string output_cloud_file, string transform_file, int minx = -1, int maxx = -1, int miny = -1, int maxy = -1, int minz = -1, int maxz = -1);

        [DllImport("pretreatment.dll")]
        public static extern void cutPointCloudapi(string input_cloud_file, string output_cloud_file, int minx = -1, int maxx = -1, int miny = -1, int maxy = -1, int minz = -1, int maxz = -1);

        [DllImport("pretreatment.dll")]
        public static extern int pcd2imageapi(string input_cloud_file, StringBuilder output_image_file, int meanK = 50, float std_dev_mul = 1.0f, int iteration = 50, int line_width = 1, int kernel_size = 10, double transE = 1e-8, double euclideanFE = 1, int planar_threshold = 15, int minx = -1, int maxx = -1, int miny = -1, int maxy = -1, int minz = -1, int maxz = -1, int min_cloud_size = 100);

        [DllImport("pretreatment.dll")]
        public static extern void transform_rotationapi(string input_file, string output_file, float rx, float ry, float rz);

        [DllImport("pretreatment.dll")]
        public static extern bool pcdRotationArbitraryAxisapi(string input_cloud_file, string output_cloud_file, float x1, float y1, float z1, float x2, float y2, float z2, float angle);

        [DllImport("pretreatment.dll")]
        public static extern bool TemplateAlignmentMatchapi(string input_cloud_file, string output_cloud_file);

        [DllImport("pretreatment.dll")]
        public static extern bool pcdAppendNormalapi(string input_cloud_file, string output_cloud_file, double radius = 30);
    }
}