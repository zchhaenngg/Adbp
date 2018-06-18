var gulp = require('gulp');
var babel = require('gulp-babel');
var livereload = require('gulp-livereload');
//var sourcemaps = require('gulp-sourcemaps');

gulp.task('babel', () => {
    return gulp.src(['./es6/**/*.js'])
        .pipe(babel({
            presets: [
                ["env", {
                    "targets": {
                        "chrome": 52,
                        "browsers": ["last 2 versions", "safari 7", "ie 9"]
                    }
                }]
            ],
            plugins: ["transform-object-rest-spread"]
        }))
        .pipe(gulp.dest('wwwroot/es5'))
        .pipe(livereload());
});

gulp.task('watch', function () {
    // Create LiveReload server
    livereload.listen();
    gulp.watch(['./es6/**/*.js'], ['babel']);
});