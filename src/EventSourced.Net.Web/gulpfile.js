/// <binding ProjectOpened='build' />
// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    babel = require("gulp-babel"),
    webpack = require("gulp-webpack"),
    uglify = require("gulp-uglify");

var paths = {
  webroot: "./wwwroot/",
  client: "./client/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.srcJs = paths.client + "src/**/*.js";
paths.binJs = paths.client + "dist/";
paths.distJs = paths.binJs + "**/*.js";
paths.binBundle = paths.webroot + "lib/";
paths.fileBundle = "bundle.js";
paths.bundleJs = paths.webroot + paths.fileBundle;

gulp.task("build:babel", function() {
  return gulp.src(["./client/**/*.js", "!./client/babel/**/*.*", "./Web/**/*.js"])
    .pipe(babel({
      presets: ["es2015", "react"]
    }))
    .pipe(gulp.dest("./client/babel"))
  ;
})

gulp.task("build:webpack", ["build:babel"], function() {
  return gulp.src("./client/babel/**/*.js")
    .pipe(webpack(require("./webpack.config")))
    .pipe(gulp.dest(paths.webroot))
  ;
})

gulp.task("build", ["build:webpack"], function(cb) {
  rimraf("./client/babel/", cb);
})

gulp.task("clean:app", function (cb) {
  rimraf(paths.binJs, cb);
});

gulp.task("clean:bundle", function (cb) {
  rimraf(paths.bundleJs, cb);
});

gulp.task("clean:js", function (cb) {
  rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
  rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:app", "clean:bundle"]);

gulp.task("min:js", function () {
  return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
    .pipe(concat(paths.concatJsDest))
    .pipe(uglify())
    .pipe(gulp.dest("."))
  ;
});

gulp.task("min:css", function () {
  return gulp.src([paths.css, "!" + paths.minCss])
    .pipe(concat(paths.concatCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest("."))
  ;
});

gulp.task("min", ["min:js", "min:css"]);
