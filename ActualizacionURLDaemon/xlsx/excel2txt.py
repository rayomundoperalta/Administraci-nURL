import pandas as pd
import time
import sys

print('Excel to txt converter')
#print(sys.argv[1:])

print('Python version ' + sys.version)
print('Pandas version ' + pd.__version__)
print(sys.getfilesystemencoding())

print(sys.argv[1])

print(sys.argv[2])

xlsx = pd.ExcelFile(sys.argv[1])

df = xlsx.parse(xlsx.sheet_names[0])
print('Cargue el archivo Excel')

with open(sys.argv[2],'w', errors='ignore') as outfile:
    df.to_csv(outfile, sep="\t")


print('Escribi el archivo txt')

sys.exit(0)