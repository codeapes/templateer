var gulp = require("gulp");
var msbuild = require("gulp-msbuild");

gulp.task("build", function() {
    return gulp.src("./Templateer.sln")
        .pipe(msbuild({
            targets: ['Clean', 'Build'],
            logCommand: true,
            toolsVersion: 4.0,
            verbosity: 'diagnostic',
            errorOnFail: true,
            pathToMsBuild: '/Users/daniel/git/templateer/backend/packages/Microsoft.Build.Mono.Debug.14.1.0.0-prerelease/lib'
            })
        );
});