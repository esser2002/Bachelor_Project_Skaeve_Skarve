set data="C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\mnist_train_2000.csv"

cd ..\NormaliseData\
dotnet run %data% "..\data\out\normalisedData.csv"
cd ..\sHDBSCAN\
dotnet run %data% "C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\dendrogram.csv" "C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\sHDBSCANStats.csv"
cd .\python\
python .\ccc.py
cd ..
