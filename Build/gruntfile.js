module.exports = function (grunt) {
    var pkg = grunt.file.readJSON("package.json");

    grunt.initConfig({
        pkg: pkg,
        msbuild: {
            dist: {
                src: ['../src/Glimpse.Nancy/Glimpse.Nancy.csproj'],
                options: {
                    projectConfiguration: 'Release',
                    target: ['Clean', 'Rebuild'],
                    stdout: true
                }
            }
        },
        clean: {
            dist: {
                src: ["./dist"]
            }
        },        
        copy: {
            nuspec: {
                files: [{
                    expand: true,
                    cwd: '../Src/Nuspec/',
                    src: ['Glimpse.Nancy.nuspec'],
                    dest: 'dist/'
                }]
            },
            lib: {
                files: [{
                    expand: true,
                    cwd: '../src/Glimpse.Nancy/bin/Release/',
                    src: ['*'],
                    dest: 'dist/lib'
                }]
            }
        },
        nugetpack: {
            dist: {
                src: 'dist/Glimpse.Nancy.nuspec',
                dest: 'dist/',
                options: {
                    version: pkg.version
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-nuget');
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-clean');
 
    grunt.registerTask("default", ["clean", "msbuild:dist", "copy:nuspec", "copy:lib", "nugetpack:dist"]);
};