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
