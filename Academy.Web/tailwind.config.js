/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml',
    './Areas/Admin/Views/**/*.cshtml',
    './Helpers/TagHelpers/**/*.cs',
    './Areas/UserPanel/Views/**/*.cshtml',
    './Areas/MasterPanel/Views/**/*.cshtml',
    './wwwroot/js/**/*.js',
    './wwwroot/client/js/**/*.js',
    './wwwroot/client/**/*.html',
    './wwwroot/shared/js/**/*.js',
    './wwwroot/shared/css/**/*.css',
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
