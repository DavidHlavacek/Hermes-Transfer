import os
import sys
file_ = sys.argv[1]
for root, dirs, files in os.walk('C:/'):
    for name in files:
        name2 = name.split('.')[0]
        extension = name.split('.')[-1].lower()
        if file_ == name2 and extension != "lnk":
            print(os.path.abspath(os.path.join(root, name)))
            break

#             #     break;






# test = "C-Doc-Ha-lol.exe"
# test2 = test.split('-')[-1]
# test3 = test2.split('.')[0]
# print(test3)
