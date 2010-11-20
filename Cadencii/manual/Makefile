all: cadencii.pdf tech_memo.pdf

tech_memo.pdf: tech_memo.dvi
	dvipdfmx tech_memo.dvi

tech_memo.dvi: tech_memo.tex tech_memo.toc
	platex tech_memo.tex

tech_memo.toc: tech_memo.tex
	platex tech_memo.tex

cadencii.pdf: cadencii.dvi
	dvipdfmx cadencii.dvi

cadencii.dvi: cadencii.tex cadencii.toc
	platex cadencii.tex

cadencii.toc: cadencii.tex
	platex cadencii.tex

clean:
	rm cadencii.pdf
	rm cadencii.dvi
	rm cadencii.toc
