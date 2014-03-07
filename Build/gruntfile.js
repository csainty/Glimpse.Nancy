module.exports = function (grunt) {
    var pkg = grunt.file.readJSON("package.json");

    grunt.initConfig({
        pkg: pkg,
        assemblyinfo: {
            options: {
                files: ['../Src/Glimpse.Nancy.sln'],

                // Standard assembly info
                info: {
                    version: pkg.assemblyVersion, 
                    fileVersion: pkg.assemblyVersion
                }
            }
        },        
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
                    src: ['Glimpse.Nancy.dll'],
                    dest: 'dist/lib/net45'
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
    grunt.loadNpmTasks('grunt-dotnet-assembly-info');
 
    grunt.registerTask("default", ["clean", "assemblyinfo", "msbuild:dist", "copy:nuspec", "copy:lib", "nugetpack:dist"]);
};