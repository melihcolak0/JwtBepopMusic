'use strict'
var gulp = require('gulp');
var requireDir = require('require-dir');

gulp.paths = {
    dist: 'dist',
};

var paths = gulp.paths;

requireDir('gulp-tasks');

gulp.task('default', gulp.series('serve'));