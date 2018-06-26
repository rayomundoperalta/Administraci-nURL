import pandas as pd
import sys

print(sys.argv[1:])

#print('Python version ' + sys.version)
#print('Pandas version ' + pd.__version__)
#print('D:\\CompraNetTemporaryDataFiles\\muestra.xlsx')
#print(sys.getfilesystemencoding())

print(sys.argv[1])

print(sys.argv[2])

xlsx = pd.ExcelFile(sys.argv[1])

df = xlsx.parse(xlsx.sheet_names[0])

with open(sys.argv[2],'w', errors='ignore') as outfile:
    df.to_csv(outfile, sep="\t")