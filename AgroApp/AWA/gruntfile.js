/// <binding AfterBuild='less' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({

        less: {
            development: {
                options: {
                    paths: ["importfolder"]
                },
                files: {
                    "wwwroot/destinationfolder/destinationfilename.css": "sourcefolder/sourcefile.less"
                }
            }
        }

    });

};
grunt.loadNpmTasks("grunt-contrib-less");