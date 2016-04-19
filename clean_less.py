#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os, fileinput,re

pattern = re.compile(r"// FORMS\n@button-font-weight:\s*bold;", re.M )


for site_num in os.listdir("App_Themes"):
    if site_num.isdigit():
        filedata = None
        f = open("App_Themes/" + site_num + "/less/variables.less", 'r')
        filedata = f.read()
        f.close()

        # Replace the target string
        print(pattern.search(filedata) )
        filedata = pattern.sub("// FORMS", filedata)

        # Write the file out again
        f = open("App_Themes/" + site_num + "/less/variables.less", 'w')
        f.write(filedata)
        f.close()
