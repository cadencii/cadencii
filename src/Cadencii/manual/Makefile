all: cadencii.pdf tech_memo.pdf

tech_memo.tex: tech_memo_scroll.tex tech_memo_synth_system.tex tech_memo_class-depend_vsq.tex tech_memo_checklist.tex

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
	rm cadencii.pdf cadencii.dvi cadencii.toc tech_memo.pdf tech_memo.dvi tech_memo.toc
