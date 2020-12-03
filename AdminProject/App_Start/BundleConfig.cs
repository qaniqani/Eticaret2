using System.Web.Optimization;

namespace AdminProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(AdminScriptBundle());
            bundles.Add(AdminStyleBundle());

            bundles.Add(ThemeScriptBundle());
            bundles.Add(ThemeStyleBundle());

            BundleTable.EnableOptimizations = false;
        }

        private static Bundle AdminScriptBundle()
        {
            var bundle = new ScriptBundle("~/main-effect").Include(
                        "~/assets/js/app.js",
                        "~/assets/js/preloader.js",
                        "~/assets/js/bootstrap.js",
                        "~/assets/js/load.js",
                        "~/assets/js/main.js",
                        "~/assets/js/fancybox/jquery.fancybox.pack.js",
                        "~/assets/js/fancybox/jquery.fancybox.js",
                        "~/assets/js/iCheck/jquery.icheck.js",
                        "~/assets/js/switch/bootstrap-switch.js",
                        "~/assets/js/footable/js/footable.js",
                        "~/assets/js/footable/js/footable.sort.js",
                        "~/assets/js/footable/js/footable.filter.js",
                        "~/assets/js/footable/js/footable.paginate.js",
                        "~/assets/js/footable/js/footable.paginate.js",
                        "~/assets/js/jquery.mjs.nestedSortable.js",
                        "~/assets/tinymce/tinymce.min.js",
                        "~/assets/js/jquery.validationEngine.js",
                        "~/assets/js/jquery.validationEngine-tr.js",
                        "~/assets/js/wizard/build/jquery.steps.js",
                        "~/assets/js/wizard/jquery.stepy.js",
                        "~/assets/js/tree/jquery.treeview.js",
                        "~/assets/js/pnotify/jquery.pnotify.min.js",
                        "~/assets/js/datepicker/bootstrap-datepicker.js",
                        "~/assets/js/scripts.js"
                      );

            return bundle;
        }

        private static Bundle AdminStyleBundle()
        {
            var bundle = new StyleBundle("~/css").Include(
                        "~/assets/css/style.css",
                        "~/assets/css/loader-style.css",
                        "~/assets/css/bootstrap.css",
                        "~/assets/js/iCheck/flat/all.css",
                        "~/assets/js/iCheck/line/all.css",
                        "~/assets/js/colorPicker/bootstrap-colorpicker.css",
                        "~/assets/js/switch/bootstrap-switch.css",
                        "~/assets/js/validate/validate.css",
                        "~/assets/js/idealform/css/jquery.idealforms.css",
                        "~/assets/js/footable/css/footable.core.css",
                        "~/assets/js/footable/css/footable.standalone.css",
                        "~/assets/js/footable/css/footable-demos.css",
                        "~/assets/js/dataTable/lib/jquery.dataTables/css/DT_bootstrap.css",
                        "~/assets/js/dataTable/css/datatables.responsive.css",
                        "~/assets/js/wizard/css/jquery.steps.css",
                        "~/assets/js/wizard/jquery.stepy.css",
                        "~/assets/js/tabs/acc-wizard.min.css",
                        "~/assets/js/tree/jquery.treeview.css",
                        "~/assets/js/tree/treetable/stylesheets/jquery.treetable.css",
                        "~/assets/js/tree/treetable/stylesheets/jquery.treetable.theme.default.css",
                        "~/assets/css/validationEngine.jquery.css",
                        "~/assets/js/fancybox/jquery.fancybox.css",
                        "~/assets/js/datepicker/datepicker.css"
                      //"~/assets/",
                      //"~/assets/",
                      );

            return bundle;
        }

        private static Bundle ThemeScriptBundle()
        {
            var bundle = new ScriptBundle("~/main-script").Include(
                        "~/theme/js/jquery-ui/jquery-ui.js",
                        "~/theme/bootstrap/js/bootstrap.min.js",
                        "~/theme/js/jquery.placeholder.min.js",
                        "~/theme/js/jquery.easing.1.3.js",
                        "~/theme/js/device.min.js",
                        "~/theme/js/jquery.browser.min.js",
                        "~/theme/js/snap.min.js",
                        "~/assets/js/fancybox/jquery.fancybox.pack.js",
                        "~/assets/js/fancybox/jquery.fancybox.js",
                        "~/theme/js/jquery.appear.js",
                        "~/theme/plugins/headroom/headroom.js",
                        "~/theme/plugins/headroom/jQuery.headroom.js",
                        "~/theme/plugins/headroom/init.js",
                        "~/theme/form/js/contact-form.js",
                        "~/theme/js/select2/select2.min.js",
                        "~/theme/js/stacktable/stacktable.js",
                        "~/theme/plugins/owl/owl.carousel.min.js",
                        "~/theme/plugins/owl/thumbnail-init.js",
                        "~/theme/plugins/owl/init.js",
                        "~/theme/plugins/color-selector/colorselector.js",
                        "~/theme/plugins/color-selector/init.js",
                        "~/theme/js/elevate-zoom/jquery.elevatezoom.js",
                        "~/theme/js/nouislider/jquery.nouislider.all.js",
                        "~/theme/js/nouislider/init.js",
                        "~/theme/js/magnific-popup/jquery.magnific-popup.min.js",
                        "~/theme/js/stars-rating/rating.js",
                        "~/theme/js/datatables/jquery.datatables.min.js",
                        "~/theme/js/main.js",
                        "~/theme/js/elevate-zoom/init.js"
                      );

            return bundle;
        }

        private static Bundle ThemeStyleBundle()
        {
            var bundle = new StyleBundle("~/main-css").Include(
                        "~/theme/fonts/font-awesome/css/font-awesome.min.css",
                        "~/theme/css/bootstrap.css",
                        "~/theme/css/select2.css",
                        "~/theme/css/jquery.dataTables.min.css",
                        "~/assets/js/fancybox/jquery.fancybox.css",
                        "~/theme/css/style.css"
                        //"~/assets/",
                      );

            return bundle;
        }
    }
}
