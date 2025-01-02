-----------------------------------------------------------------------------

ISTRUZIONI PER LA CONVERSIONE DEI FILES IN .TXT IN FORMATO UFT-8 - NON SPOSTARE DA QUA, IN TESTA AL README (PROVENGONO DA TEXTANALYZER 010 016)

- Se possibile partire da files nativi UTF8, e curati come uso della punteggiatura (ad averli...)
- Il Notepad di Windos è utilissimo perchè consente di aprire un .text con una codifica qualsiasi e poi di salvarlo con nome scegliendo il tipo di codifica:
		scegliere UFT8 o, ancora meglio, UFT8 con BOM (casellina appena a sinistra del pulsante 'salva')
- Word invece è una scocciatura perchè non permette di scegliere la codifica con cui salvare i files .txt. [Non so se salva sempre in UTF7 o se
		la codifica di salvataggio dipende dalla codifica del file .doc/.rtf originario]. Temo che la cosa migliore sia aprire il file .doc/.rtf, selezionare
		tutto e poi copiarlo in Notepad. Oppure salvare il .txt e poi, se causa problemi, aprirlo con NotePad e risalvarlo con la codifica giusta come detto sopra.
- Files .pdf: Acrobat sembra non abbia 'seleziona tutto', ma invece c'è, tramite lo shortcut Ctrl-A (che funziona anche in Word e Notepad)! Nota: su certi .pdf
		la copia Ctrl-C non funziona perchè l'autore ha bloccato la copia... stronzi. Nemmeno Word riesce ad aprirli. Con certi altri il Ctrl-C funziona, ma
		quello che si ottiene è un jumble di caratteri incomprensibili (anche aprendoli con Word). Hahah la cosa divertente è che ne ho trovato uno (Cesare Beccaria,
		dei delitti e delle pene) che così ad'occhio sembrava un cifrario a sostituzione semplice... l'ho salvato e comparato e, looll, è proprio un cifrario hahahaha
		(altri pdf usano encryptions un pò più robuste, superstronzi xD)

	Ricordare che un messaggio di errore avvisa quando, aprendo un file, si incontrano caratteri 'strani', che molto probabilmente sono dovuti ad un problema di codifica
	del .txt (il programma prova automaticamente UFT8 e UFT7, ma anche così si trovano files UFT7 in cui certi caratteri (p.es. quelli accentati) diventano caratteri
	di controllo o altre stranezze). Aprire il .txt col Notepad e salvarlo UFT8+BOM come detto sopra.

	Il controllo finale lo si fa comparando il file col corpus della sua lingua e controllando l'Additional Report che elenca le parole frequenti nel testo ma rare nel corpus,
	in modo da vedere se ci sono parole troncate o lunghe una sola lettera o simili! Con questo metodo si trappano anche problemi dovuti al testo in sè stesso e non alla sua
	codifica, per esempio ho trovato un.pdf che conteneva cose come 'Voi soli vo[i]rreste morire' o 'l<i> hanno tagliato', che non è ovviamente possibile trovare o
	correggere automaticamente,	ma che possono essere identificati con l'Additional report (se p.es. compaiono parole come 'vo' o 'rreste')

	E RICORDARE DI INCLUDERE I COMANDI DI PREPROCESSING quando si fa un'analisi, evita di perdere una cifra di tempo per capire come mai compaiono parole
	'strane' (che capita quando p.es. il testo ha usato un carattere non standard per l'apostrofo (tipicamente '’') che viene corretto dai comandi Regex di pre-processing)!!!!


 ISTRUZIONI PER L'USO DELLE OPZIONI DI PRE-PROCESSING

	In generale NON vanno modificate rispetto ai valori di default, questo perchè le opzioni usate influiscono sulle statistiche e potrebbero verificarsi dei problemi
		comparando, per esempio, files generati con opzioni di preprocessing diverse. Dato che l'idea di base di TextAnalyzer è di poter processare files indipendentemente
		dalla lingua in cui sono stati scritti è molto meglio usare sempre le stesse opzioni, anche se per casi particolari è pur sempre possibile modificarle.

			- Se si cerca di comparare files generati con opzioni diverse viene dato un warning. E' solo un warning perchè, dopotutto, non è detto che opzioni di pre-processing
				diverse	per caratteri quali dash o apostrofi spostino le statistiche talmente tanto da alterare i risultati delle comparazioni.
		
		Le opzioni migliori sono:

			Dash '-': discard words. Inutile stare lì a farsi le seghe mentali... unire le parole separate da dashes è carino, ma causa la comparsa di parole
										assurde quando l'autore del testo ha usato il dash enfaticamente in questo modo: scarpe-antisdrucciolo-di-cuoio-del-nord-dakota
										(esempio reale...), che creerebbe la parola scarpeantisdrucciolodicuoiodelnorddakota da 38 lettere... (in effetti era ancora più
										lunga... 54 lettere totali, ma non me la ricordo tutta xD). Anche separare le parole coi dashes è carino, ma causa la comparsa
										di parole strane, per esempio i-nu-til-men-te (enfatico) crea i, nu, til, men e te. Eliminarle senza pietà, che tanto sono poche!

			Apostrofo ''': l'opzione 'considera l'apostrofo come un carattere normale' è la migliore, per quanto abbia i suoi difetti:

					Hawaiano: perfetto, dato che l'apostrofo è davvero un carattere (stop glottale)
					Inglese: recupera "don't", "i'm", "won't" come parole, il che è bene perchè sono molto comuni. Purtroppo recupera come parole
							   anche i genitivi sassoni come "john's" e "mary's": bisogna farsene una ragione.
					Olandese: credo che vada bene sempre, recupera come parole "'t" e "s'hertogenbosch"
					Italiano: ortografia rognosissima... vengono recuperate come parole "l'amore", "dell'evento", "un'oca" etc., il che non solo allunga
								inutilmente il vocabolario, ma toglie anche le occorrenze apostrofate di "amore, "evento", "oca" etc. dalle statistiche,
								che è anche peggio. Ma dopo averci pensato su bene, ne ho dedotto che non c'è niente da fare:
									1) usare sempre le stesse opzioni per tutte le lingue è TROPPO importante (per capire provare a rispondere alla domanda: che
										opzioni uso per una lingua sconosciuta?)
									2) le alternative sono:
											2a) Unire le parole: ne crea di altrettanto assurde, anzi peggio, che se l'apostrofo fosse stato mantenuto ("lamore", "dellevento", "unoca")
												e allunga il vocabolario e altera le statistiche allo stesso modo che mantenendo l'apostrofo. Inferiore alla 1) da tutti i punti di vista.
											2b) Eliminare le parole sarebbe non male: evita l'allungamento del vocabolario e non inserisce parole strane, ma altera le statistiche di
													"amore, "evento" e "oca" allo stesso modo dell'opzione 1).
											2b) Separare le parole sarebbe buona: ne crea di assurde ("l", "dell") ma allunga di poco il vocabolario e, soprattutto, non altera le statistiche
												di "amore, "evento" e "oca". Si potrebbe usare per un'analisi specializzata per l'Italiano, ma la considerazione 1) è troppo importante
												se si vuole avere una gestione davvero language-indipendent!

					PS.: temo che il Lombardo Orientale sarebbe anche peggio dell'Italiano...

	NOTA

		Ci sono delle piccolissime differenze inevitabili fra le frequenze (p.es. bigrammi) calcolate tramite analisi di testi multipli e tramite aggregazione
			di analisi singole, questo perchè concatenando tutti i files in un'unica stringa vengono generati dei bigrammi aggiungivi (p.es. SPAZIO-lettera)
			nel punto in cui due files vengono concatenati. Poco male, ma siccome può generare confusione ho inserito questa nota anche nelle istruzioni in testa
			al readme. Probabile anche che, finito il debug, limiterò ad un solo file l'analisi dei testi.



	OSSERVAZIONI

		OSSERVAZIONE: i testi troppo corti non funzionano molto bene con le Comparazioni (c'era da aspettarselo). Cosa vogla dire 'troppo corti' non so, ma direi fore anche lunghi meno di 15K characters
			(che non sono pochi...)

		OSSERVAZIONE: i bigrammi fanno un discreto lavoro per identificare l'autore di un testo quando si comparano i testi, ma non c'è da pretendere troppo
			(e occhio ai testi troppo corti come detto sopra) P.es. Alfieri Vittorio - Filippo è vicino a tutti gli altri Alfieri, eccettuato Della Tirannide
			che è un outlier. Argomento da approfondire.
	    
		OSSERVAZIONE: usando tutti i caratteri invece che limitandoli col rare_characters_cutoff le statistiche unblinded in-cluster dei bigrammi migliorano lol
			(quelle out-cluster non cambiano significativamente). Anche altre statistiche si comportano allo stesso modo mi pare ma non ho fatto un'analisi sistematica
			(ho poi visto che non accade sempre così, nemmeno coi bigrammi, p.es. i testi di Dante si allontanano togliedo il rare_charcters_cutoff).

		OSSERVAZIONE: la statistica vocabulary blind distingue Voynich biological dal completo (in tutte e 3 le trascrizioni), che e' un'eccezione perche' in generale
			vocabulary blind non distingue affatto fra le lingue. Vedere il grafico 2d comparando i 6 voynich bio/completo nelle tre trascrizioni,
			Fra lingue 'vere' la distanza max vocabulary blind (fra De Bello Gallico e Midway Revisited) è 0.0468, la minima (fra Deledda - Dopo il Divorzio e
			Salgari - La regina dei caraibi) è 0.00588. Invece la distanza di Voynich EVA Biologial da Voynich EVA Completo è 0.039 (simile a De Bello Gallico - Canterbury tales 0.042
			o De Bello Gallico - Divina Commedia Inferno 0.037). Invece le altre statistiche raggruppano in tre clusters diversi le tre trascrizioni del Voynich, anche se credo
			con distanze sempre	sopra al limite accettato per considerarle la stessa lingua(ma è da verificare!!!)

------------------------------------------------------------------------------------------------------------------------------------------

VOYTEXTANALYZER 010 014

OBIETTIVI

	Gli scopi di questa versione sono (1) riorganizzare un po' il software perchè le ultime aggiunte lo hanno incasinato e (2) inserire una codifica con Huffmann
		codes per i chunks e i word tokens suddivisi in chunks per vedere se così il #totale di bits richiesti ha senso come metrica per le grammatiche.


SVILUPPO

  Iniziato il 16 Dicembre 2024


	16/12/2024 riorganizzato il software aggiungendo a Voynich_parsing_results alcune variabili e classi che prima erano calcolate in Form1 (e adesso sono state
		spostate in ParseVoynich mentre in Form1 sono restate solo le visualizzazioni vere e proprie)

	16/12/2024 verso ora di cena. Wow, inserita tutta la codifica Huffmann :) Niente che sono fico xD E... pare che funzioni come metrica!!!!!!!!!!!!!!!! LOOP-Lay is the best at the moment lol

	17/12/2024 inserita una checkbox per la visualizzazione delle chunk categories. Eliminato il calcolo del numero di bits con codici a lunghezza fissa e rivisto il formato del report.
				Aggiunte scritte nella status window per le gestioni 'parse' e 'asemic'. Aggiunte intestazioni alle textBoxes.

	19/12/2024 aggiunta textbox per programmazione seed del random number geerator per Asemic. Inoltre il numero di parole che vengono generate non è più un parametro ma 
				viene calcolato.

	19/12/2024 aggiunta la disabilitazione e riabilitazione dei controlli sulle funzioni aggiunte (Parse text, Parse text with WSET, Write asemic). Usano le solite
				disable_controls_while_threads_are_running() e enable_controls_when_thread_stops() anche se in effetti queste funzioni non sono in una thread

	19/12/2024 aggiunto un test per trappare p.es. il caso del testo "ab ba" con la 'b' che manca nella grammatica. Il programma crasshava perchè non viene trovata nemmeno una parola


CHIUSURA

	19 Dicembre 2024




BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO (IN MASSIMA PARTE PROVENGONO DA TEXTANALYZER 010 016)

			- In ParseVoynich ho un parametro non programmabile: min_occurrences, che non fa elaborare i word tokens che compaiono meno di di min_occurrences.
				Non è particolarmente utile però. Evitare di lasciarlo != 1 nel software, riportarlo sempre a 1 dopo Eventuali prove! (e se venisse
				gestito andrebbe scritto nel report!!!) POCO UTILE

			- Stranezza delle distanze dei clusters: inspiegabile senza un disegno, metterla nella documentazione XPlotClusters!!! (spero di averlo fatto... 19/12/2024)


	- GROVIGLI SOFTWARE

		Sono, ahimè, il primo problema

		1) Problemi con le threads in generale (altra bella menata). Qua seguono alcuni appunti:

			 Find best matches andrebbe messa in una thread per evitare un crash dovuto al timeout se si scandiscono moltissimi files (mai accaduto fin'ora però). Notare che anche
				la parte iniziale di load_and_analyze (dove vengono caricati i files, prima di lanciare la text_analyzer_main) potrebbe avere lo stesso problema. Inoltre
				anche la gestione di FormCompare andrebbe messa in una thread, adesso se si cerca di comparare moltissimi files (~300) si va in timeout (o il programma si blocca, se non si
				usa il debugger). Mass analyze sembra non abbia problemi anche se non è in una thread, forse sono le scritture dei files che salvano dal timeout? Mass aggregate 
				non è in una thread, non l'ho mai vista andare in timeout (con 309 files) ma... Notare che sulle funzioni che non sono state inserite in una thread, ovviamente, 
				non viene lanciata la disable_controls_while_threads_are_running. Menù e pulsante restano abilitati ma pare non ci si possa accedere. Boh... interessante comunque.
				
				Sempre parlando di threads...:

					In mass_analyze c'è un error.Display_and_Clear che TEMO vada lanciato con un Invoke o provocherà un'eccezione.
					
					Ho modificato xstandards.mdfile.save, dove ho dovuto modificare due accessi	alla main_status_window aggiungendo gli Invokes
						(mentre un accesso con newline_to_main_status windows in una gestione di errore non è stato modificato, e anche qua c'è un
						error.Display_and_Clear). Ma... MI SONO ACCORTO DOPO che esiste già un delegate per il .save... il che vuol dire che avrei dovuto
						lanciare il .save con un Invoke, senza modificare la routine standard.

				Sempre parlando di threads...:

					Tutte le gestioni inserite con VoyTextAnalyzer (pulsanti Parse text, Parse text with WSET, Write asemic) non sono sotto thread.
						Però le elaborazioni sono veloci (per lo meno con testi corti), più che altro il tempo è usato dalle visualizzazioni
						(che non richiedono una thread per non andare in timeout, almeno credo)

		2) Per quanto abbia separato decentemente calcoli da visualizzazioni ci sono ancora dei casini.
				
				Sarebbe utile per la maneggevolezza del software spostare il grosso delle routines di visualizzazione da Form1 a delle classi opportune.
				
				In FormCompare, contrariamente a tutte le altre functions di questo genere, display_report e display_additional_report non hanno una variante calculate_ e una display_
				avevo impostato male la cosa fin dall'inizio e la gestione del calcolo è troppo inframmischiata a quella della visualizzazione perchè valga la pena di sistemarla adesso

				In FromCompare bisognerebbe eseguire il doppio calcolo visualizzazione senza clustering/visualizzazione con clustering in calculate_2d_data/calculate_1d_data,
				 da dove andrebbe tolto il test checkBox_Compare_show_clusters_CheckedChanged (e testando la checkbox solo in visualizzazione ovviamente). E' CHE E' UNA PALLA....
				 SONO UNA CIFRA	DI VARIABILI DA AGGIUNGERE E DI CASI NELLE ROUTINE DI VISUALIZZAZIONE.... E IL TEMPO DI ELABORAZIONE COMUNQUE E' ABBASTANZA VELOCE, MAH...

				NOTARE: in FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
				assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpagliate quà e là. Non l'ho sistemato in Form1perchè c'è il rischio di
				infilarsi in qualche problema di Invoke, MA ME NE DEVO RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
				sono scritti meglio, USARE QUELLI PER IL TEMPLATE!! NOTA: FormClustering E' PIU' SEMPLCIE DI FormCompare E LI' CALCULATE_DATA/DISPLAY_DATA SONO SEPARATI PER
				BENE, RIFERIRSI A FormClustering PER QUESTA PARTE

		3) Per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
			'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
			MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
			I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE. AHHHH E I NOMI GIUSTI SAREBBERO EV_float, EV_long, EV_float_2d etc.!!!
			Inoltre ho tante di quelle varianti delle EVconvert() che non è facile starci dietro......

				IN GENERALE, GLI EVALUE SERVONO +- A QUALSIASI SOFTWARE.. PENSARCI ANCHE PER IL TEMPLATE. E quando li si usa, sarebbe meglio usare classi _derivate_ dagli
					EValue, con un loro nome specifico, per evitare confusione.

		4) Ho quintali di codice duplicato un po' dappertutto (in particolare nelle visualizzazioni, ma non solo)

		5) Bugs:

			Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
			il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
			FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

			Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
			finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
			semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
			chiusa se si lancia una nuova analisi o un nuovo compare...  IN GENERALE, IL DISCORSO DI COSA DISABILITARE E QUANDO ANDREBBE RIVISTO COMPLETAMENTE

			Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.

		6) Bug in XPlotClusters, che però non può mai verificarsi per come calcolo FormClustering.clustring results: sicuramente c'è la possibilità di sovrapposizioni di
			bounding_boxes in XPlotCluster.find_best_position e find_best_placement in casi sfigati. Vedere Documentazione XPlotClusters.docx, nella directory /bin/debug,
			dove spiego la cosa con un disegno.

		7) Debito tecnico. In XPlotClusters find_numbers_positions cercare // QUA BISOGNEREBBE TOGLIERE L'AVAILABLE_POINT UTILIZZATO, ED AGGIUNGERE  I DUE POINTS SUCCESSIVI,
				QUESTO PER UNIFORMITA' COL RESTO DELLE GESTIONI    //    E PER EVENTUALI ESPANSIONI FUTURE
		   
	   

	- NUOVE FUNZIONALITA'


		- Eliminare la possibilità di analizzare più di un file di testo alla volta ha risolto un bel problema [Mi consente di settare le variabili loaded_files_list.file_charset_size e .file_max_word_length sui valori dell'ultimo
			(e, adesso, unico) file caricato, mentre prima, coi multifile, restavano a zero (solito problema di non poter sapere dove un file inizia o finisce)] ma ha il
			difetto di non poter più usare un file di comandi di preprocessing (che potrebbe essere comodo). Si potrebbe semplicemente alzare il limite a due
			files (spiegando almeno nel manuale che il preprocessing deve essere il primo file (in ordine alfabetico, hmm testare che il nome inizi con '!'??) e di
			non includere altro pena bug nel valore delle variabili di loaded_files_list).

		- Se si vuole trovare le parole inusuali in molti testi la cosa è molto scomoda, dato che ogni volta bisogna caricare il reference e poi scegliere un corpus con
			cui comparare. Sarebbe bello introdurre una funzione 'Compare with corpus...' che eviti di dover selezionare ogni volta lo stesso file. Serve una variabile per
			il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova nullo, che direi di salvare in config e di visualizzare in options.
			UTILE!

		- Funzioni 'Analyze and Check': come le Analyze, ma esegue anche una comparazione col corpus di riferimento e visualizza la finestra
			additional report, in modo che si possa verificare subito se si vedono parole troncate o strane che fanno supporre problemi di ortografia
			nel testo sorgente. Serve una variabile per il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova
			nullo quando si inizia un' Analysis and Check, che direi di salvare in config e di visualizzare in options. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

				Dopodichè potrei fare una "Mass analyze, check & save": similmente a Mass Analyze, per ogni file .txt lancia l'analisi, poi la comparazione
								col corpus di riferimento e visualizza la pagina parole inusuali. Poi servono due pulsanti: 'accept' salva il file .txanalysis con lo
								stesso nome del .txt, 'don't accept' modifica il nome su disco del .txt incriminato per revisione successiva. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

		- Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
			SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
			FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
			nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
			SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

		- Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
			PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO



	- MIGLIORIE

		- !!!!! Eh, dopo aver introdotto gli Huffmann codes in VoyTextAnalyzer 010 014 mi è venuto in mente che una struttura ad albero come l'Huffmann tree sarebbe 
			l'ideale per l'algoritmo di clustering!!! E' che non è cosi' semplice: il problema è che quando riunisco due nodi dell'albero (che sarebbero due clusters) non posso
			semplicemente sommare le loro due distanze... dovrei calcolare le distanze del nuovo nodo da tutti gli altri... il che appare disastroso per il tempo di elaborazione...
			a meno che non ci si possa inventare qualcosa. Molto interessante però, otterrei un clustering 'perfetto', contrariamente a quello che ho oggi.

		- Nella pagina 'Compare' 'Unusual words' le parole sono ordinate per frequenza, ma dopo bisognerebbe ordinarle anche per # occorrenze. Altrimenti quando ci sono
			p.es. centinaia di 'never-found before words', tutte con la stessa frequenza, la lista è disordinata per occorrenze e causa una gran confusione
			(p.es. confrontando gli hapax legomena del Voynich e di AsemicVoynich). Solo che è menoso... dovrei trasferire tutto in una lista di liste dopo il primo
			ordinamento per poterle poi ordinare anche per # occorrenze. Cercare ""ORDINAMENTO LISTA UNUSUAL WORDS" in FormCompare

		- Aggiungere nella tab 'Slot Grammar', visualizzazione 'chunk categories' la distribuzione complessiva dei chunks verso # di utilizzi categoria per categoria.
			Complicata dal punto di vista della visualizzazione.

		- Form1.findAnalysisClusters non gestisce 'preprocessing options are different' 

		- Opzione per una scala 'assoluta' del clustering? E' che deve cambiare da statistica a statistica...
		- Varie ed eventuali (titolo e scala colore XplotClusters)
		- XPlotClusters: sarebbe comodo un Ctrl-F nel grafico.. e un modo per copiarlo (dato che è enorme)
		- Mousehover clusters: scrivere il nome completo del file da qualche parte

		- Sarebbe bello, nel caso sia attiva l'opzione "discard words containing apostrophes", creare una lista della parole rimosse. Questo renderebbe più utilizzabile
				l'opzione (che forse si potrebbe usare di default), eliminando un bel po' di parole 'spurie' p.es. dal vocabolario Italiano.

		- Sarebbe bello un 'reverse colors' per i grafici 2D....

		- Rimozione delle abbreviazioni monoletterali: purtroppo non è facile rimuovere anche abbreviazioni come q.o.d. o S.P.Q.R., questo perchè il replace Regex che uso elimina
				tre chars in totale (il carattere non-letterale che precede la lettera, la lettera e il punto che la segue), col risultato che p.es. " q.o.d." diventa " o " dopo
                il primo passaggio (ne faccio due in preprocessing), e la 'o' non viene eliminata. Dovrei rimpiazzare solo due caratteri per l'abbreviazioni, cosi' " q.o.d." diventa
				"  o. " e al secondo passaggio verrebbe eliminata anche la "o.", ma non so come fare con Regex e sinceramente non ci voglio perdere tempo adesso.
					PS.: un abbreviazione non viene eliminata se è a inizio file. POTREBBE ANCHE ESERE CONSIDERATO UN BUG (BTW E' RIPORTATO NEL MANUALE)
	
		- Mass analyze and save: ad ogni file viene ancora aggiornata la trackbar delle tables_sizes (tutto il resto delle visualizzazioni è stato
				eliminato per velocizzare l'esecuzione)... ma dato che c'entrano i graphs_limits sistemarlo può essere rischioso (E ANCHE POCO UTILE)

		- Find best matches non genera dei 'bei' grafici 2d di riepilogo, ma questo perchè, trovando testi a distanza ravvicinata, la scala Z dei grafici è molto
			espansa ed evidenzia anche le minime differenze nei testi. Bisognerebbe poter aggiungere un'outlier... ma va visto bene come sceglierlo
			(p.es. che abbia una distanza del doppio? del testo più distante fra i 24 selezionati? (ci si può anche annotare la lista dei testi
			e poi fare un Compare manuale aggiungiendo anche un outlier... ma selezionare i 24+1 testi per il Compare è una menata più unica che rara).			

		- Nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		- Find best matches usa sempre il default di rare_characters_cutoff. Renderlo programmabile anche per find best matches non è semplicissimo perchè
				non esiste una textBox che lo può fare (nè ha senso aggiungerne una globale in Analisi per solo questa funzione). Chiedere il valore quando
				si lancia find best matches sarebbe una scocciatura quando lo si usa. Forse un'opzione sarebbe meglio. Anche max_output_results è fisso
				(viene dichiarato nella getsione di find best matches, evento Click su findBestMatchesInToolStripMenuItem). Anche qua un'opzione sarebbe credo l'unica
				soluzione sensata.

		- hmmm... se nessun cluster unblinded ma ce n'e' uno dubious sarebbe bello fare l'analisi unusual words su quello... MA OCIO PERCHE' HO RICHIAMATO
			   PER NOME I DATI DI BASE (_unblinded etc.) IN TANTI PUNTI MI SA....... E CI SARANNO PROBLEMI ANCHE CON L'IDENTIFICAZIONE DEL FILE
			   DA COMPARARE PROBABILMENTE... STARE ATTENTI...

		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica ACCESSORIO
			Come accessorio alla decrittazione  vedere se può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto...).
				Magari nel report? L'HO SCRITTO NEL MANUALE

		- Visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tutti i casi visualizzare solo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE MA TUTT'ALTRO CHE INDISPENSABILE
			

		- In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). Tabellina da alcune prove fatte:

								Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

					BESTIA DI MODIFICA CHE COMPLETEREBBE IL SOFTWARE, MA CHE HA COME PREREQUISITO LA SISTEMAZIONE DEI GROVIGLI, SENNO' E' UN SUICIDIO

					Ricordare che è utile anche per la funzione 'Clustering' (ma in questo caso il tipo di statistica andrà scelto al momento del lancio del
						clustering, per non impiegare eoni di tempo di elaborazione)


	- In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina PER ORA HO INSERITO L'AVVISO NEL MANUALE
					
	- Avviso nell'additional reporty vocabulary su cosa fare se si vedono parole strane (c'è il file di pre-processing per eliminare i caratteri fallati? se c'è ma non
		funziona è un problema di codifica: aprire il file e sostituire manualmente i caratteri fallati. se sono proprio fallati in origine nel file buttarlo via..) 
		IL MAX SAREBBE UN PULSANTE DI HELP LOCALE... MA ADESSO NEL MANUALE SPIEGO PER BENE TUTTO L'ARGOMENTO

	- Analisi/vocabolario: inserire un numero progressivo quando visualizzo le parole del vocabolario per le parole. NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA
			text_analyzer.analysis_results.vocabulary_words_distribution in un EValue_extended per aggiungere nell'element_additional il numero progressivo. la lista
			è lunga ci si può impiegare del tempo. BOH, FORSE E' UNA COSA CHE NON SERVE NEANCHE POI TANTO, E COMUNQUE ANDREBBE DOPO AVER SISTEMATO UN PO' DI GROVIGLI
			
	- Il vowel_set è una menata... e si allunga sempre più... non ci sarà una function 'di sistema' per capire se un carattere è una
			vocale o una consonante? mi pare di aver cercato e non era facile.... e poi è tanto lavoro per le pseudosillabe... anche se sono carine hehe	     

	- Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH... GRAN LAVORO MENOSO PER
		POCO RISULTATO MI SA (ALMENO AL MOMENTO)

	- Grafici 1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE

	- Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	- XGraphics1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	- XGraphicsd: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	- XGraphics2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	- XGraphics2d e 1d: eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...

	- Mancano un mucchio di pulsanti 'Select all', e inoltre c'è il famoso bacherozzolo del .Select: se si è già sulla textBox col cursore non funziona!

	- Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del tipo di grafico
		visualizzato (1d o 2d),	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMENTE IMPORTANTE
		NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS', E LA COSA PUO' CREARE
		CONFUSIONE Già che siamo in argomento: sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
		E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
		cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
			NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
			LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)	

	- Inserire trackbars + caselline numeriche per settatura limiti min e max della scala Z dei grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	- Nella pagina Analisi/Chars Distances potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter se voglio anche far vedere i grafici lineari,
	   ma chissenefrega, potrei continuare a visualizzare i garfici 2d ma esportare nella textBox i dati 1d per poterli poi copiare in Excel ). Per uniformità andrebbe fatto anche nella
	   previous/following.... E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUZIIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	   DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA... en passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	   sia il caso di aggiungerli causando infiniti problemi di compatibilità coi files .txalysis) . BOH... NON SO QUANTO SIA UTILE, VEDERE

	- Pulsante e casella di testo per settare il limite dei grafici 2d anche nella pagina Analisi usando il rare_characters_cutoff come si fa col Compare
		(e salvataggio del file .cfg del rare_character_cutoff). OVVIAMENTE MANTENERE ANCHE LA TRACKBAR E LA CASELLINA DI TESTO CHE CI SONO ADESSO, E' SOLO
		UNA FUNZIONE AGGIUNTIVA!!



SCARTATE:

	Sarebbe proprio bello avere anche il il Compare-Report 2d, oltre che l'1d. MA NO! SERVE A NADA, BASTA ANDARE A VEDERE LA PAGINA BIGRAMMI E LI' C'E' GIA' IL
				GRAFICO 2D DESIDERATO.. CON TUTTE LE LINGUE IN COMPARAZIONE RAFFRONTATE FRA LORO.

    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... EVITARE COME LA PESTE...
	 MODIFICARE LE OPZIONI FRA UN FILE E L'ALTRO PUO' CAUSARE CASINI CON LE STATISTICHE (C'E' UN AVVISO APPOSITO) Mancano anche le altre opzioni che avevo immaginato
	 (set author, set title...) ma non so se valga la pena di implementarle davvero. DIREI PROPRIO DI NO! EXTRAMENOSE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA ANCHE PERCHE' I CRITTOGRAMMI VENGONO GIA' RICONOSCIUTI BENE, DIREI PROPRIO DI LASCIAR PERDERE

	Migliorare il sort delle tabelle dati 2d di riepilogo in FormCompare (i grafici colorati rosso-giallo-verde) in modo da far vedere meglio i clusters. INUTILE NEANCHE PROVARCI,
		IL PROBLEMA DI FONDO E' CHE I CLUSTERS SONO SPESSO TROPPO COMPLICATI (CON OVERLAPS FRA DI LORO) PER POTER ESSERE RAPPRESENTATI IN DUE DIMENSIONI. SERVE UN APPROCCIO
		DIVERSO 8CHE TENTERO' CON LA FUNZIONE SPECIALIZZZATA DI CLUSTERING)
	
	Dato che le statistiche delle comparazioni dipendono dai graphs-limits, si potrebbe provare a modificare automaticamente i limiti per vedere se si ottengono matches migliori?
	 Mah, può essere un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. DIREI UN GROSSO LAVORO CON TANTI 
	 POSSIBILI PITFALLS (IN PRIMIS IL TEMPO DI ELABORAZIONE, SUL QUALE SI POTREBBE FARE POCO) PER VALORE AGGIUNTO FRA LO SCARSO E IL NULLO



-------------------------------------------------------

11/12/2024 inizio v010 013

	11/12/2024 Eliminata la 'tail' (ed i cadaveri della gestione 'head'). Inoltre adesso le grammatiche sono dei veri e propri loop dove si può settare il # max di ripetizioni

	11/12/2024 Rivista la struttura software delle visualizzazioni. Aggiunti Nchunktypes, Nchunktokens e Ntransitions al report. Eliminati i numeri di debug dalla
				textbox delle chunkified words, al suo posto inserite le liste NOTFOUND e RARECHARS. Inserita statistica word _tokens_ coverage.

	12/12/2024 Adesso i rare_characters (e i rare_chars_groups) sono definiti all'interno della grammatica per maggior flessibilità (es: chunkificazione linguaggio naturale)

	12/12/2024 Aggiunta casella di testo per poter settare il numero massimo di ripetizioni di una LOOP grammar

	12/12/2024 aggiunto al report la distribuzione word types vs. number of chunks

	12/12/2024 inserito un avviso se si cerca di fare una Parse Text senza avere alcuna analisi caricata

	14/12/2024 in Asemic, durante la creazione di cumulative_transitions_table_list, adesso pre-ordino gli elementi in transitions_table_list. Non cambia niente
				dal punto di vista software (non era necessario per l'algoritmo Asemic), ma rende più facile l'interpretazione della classe usando il debugger,
				dato che gli stessi dati vengono ordinati quando li si visualizza in Form1.

	15/12/2024 dopo la tragica scoperta della grammatica trivial CSET looppata, che manda a puttane la metrica Nchunks, aggiunti il calcolo
				di DICTIONARYbits e TEXTbits, peccato che non vada bene nemmeno questa come metrica :(((( Forse mi salvo solo con un Huffmann coding... forse...


		AMMESSO DI RISOLVERE IL PROBLEMA DELLA METRICA...

				---> trovare la LOOP grammar migliore : PROBLEMA SEMPRE APERTO ::(((


	RICORDARE il min_occurrences in ParseVoynich!!!! (DOVREBBE ESSERE SCRITTO NEL REPORT!!! ma non è programmabile, non l'ho inserito per non fare troppo casino
			con una casella di testo in più, evitare di lasciarlo != 1 nel software, riportarlo sempre a 1 dopo Eventuali prove!)
						
		e anche il random_seed è modificabile solo in compilazione

		e bisognerebbE proprio aggiungere gli interbòlocchi prima di rilasciare il software.....

		e qualche scritta nella status window....

		NELLA PAGINA 'COMPARE' 'UNUSUAL CHARACTERS' bisognerebbe poter ordinare anche per il # occorrenze, non solo per frequenza (ma la differenza dove sta? indagare...)
			xkè col Voynich (e in generale quando ci sono solo parole unusual mai trovate prima) è molto confusionaria. In effetti il max sarebbe ordinare prima per
			frequenze come adesso, e dopo per # occorrenze (il che spiega anche la 'differenza': per occorrenze serve solo quando le frequenze sono tutte uguali :) )
			cercare // ORDINAMENTO LISTA UNUSUAL WORDS  in FormCompare

		

		


	ANCORA NON HO:

				  distribuzione complessiva dei chunks verso # di utilizzi
				  distribuzione complessiva dei chunks verso # di utilizzi categoria per categoria

					ENTRAMBE belle, ma dove vado a scriverle????


----------------------------------

11/12/2024 ok, congelo VoyTextAnalyzer così com'è, visto che ho ottenuto risultati direi molto interessanti

Passo alla 010 013, dove il primo obiettivo è rendere il programma più maneggevole, in particolare getsione rare characters, gestione 'y' nella tail, visualizzazioni...

Poi vedrò di trovare la grammatica con meno chunks che riesco a trovare (scusate la ripetizione..), visto che Cinitials con 'y' spostata nello slot C sembra assai promettente...

---------

06/12/2024 inizio v010 012


	HAHAHHAHA GESU', CHE MITOOOOOOOO!!!!! 07/12/2024


			ANCORA NON HO la distribuzione complessiva dei chunks, nè quella dei chunks aordinati per categorie (andrebbe fatto nella tab Parse Voynich.. ma
				il casino è l'output.... una textbox in più??? eliminare i numeri di debug per fare spazio sul video???)

				LOOP-4 headless: 606 chunks, 8165 transizioni, coverage 99.28%

				Zattera Basic richiede 2847 chunks..
				Zattera2x 3261 chunks... circa 11K transizioni (e ha covreage più basso, 93.2% xD)

				ThomasCoon_v2 richiede 3884 chunks
				ThomasCoon2x_v2 richiede 4411 chunks...  e 10.5K transizioni (ma coverage 82.8% xDD)

				Ergo... ho un modo per dire quale grammatica è la migliore hahahahahahahahahahah
				hMMMM.... loop-2 HEADLESS: PARAGONABILE CREDO A xATTERA E cOONS bASIC!!!

		1) WRITER, perchè è indispensabile per validare tutti i calcoli direi. Gestire la scelta dei chunks su numero casuale non è banale.. serve del lavoro
				Struttura ad albero!! Vedere 
					OKKKKKKKKKKKKKKKKKKKKK!!!!!!! 07/12/2024
	

			// ATTENZIONE: total_number_of_chunks non include il chunk finale con la 'y', per cui se 'y' non compare da nessun'altra parte
            //              (caso che non si verifica con LOOP, Zattera e ThomasCoon) bisognerebbe aggiungere 1 [in Form1]
	

	hmmm lol... provare a mettere giù una grammatica basata sulla struttura delle sillabe per chunkificare un linguaggio naturale? HAHAHA LOOOL FICHISSMO :) 09/12/2024

			eh cristo dio serve che i rare-characters sianod efiniti all'interno della grammatica.... oh yes!!!!!!!!!!!!!!!
			e anche la storia dello slot 'y' finale obbligatorio andrebbe rivista... non vorrei p.es. che scasini i risultati per, p.es., grammatica
			LOOP-4 dove però aggiungo una 'y' allo slot 'consonanti'... da vedere direi  !!!!!!!!!!

	note: LOOP-4 Cinitials ha Nchunktypes = 609 chunks invece dei 606 di LOOP-4, ma Nchunktokens = 1166 mentre la LOOP4 standard ha 1223 (quindi sarebbe peggiore con questa metrica)
			E ha anche un bel po' meno transizioni: 6820 vs. 8165...	ma è anche vero che il coverage scende da 99.2% al 95%. Vai a sapere te.

			e Cinitials con 'y' spostata nello slot C: 531 chunks (!), 1026 tokens (!), 6229 transizioni (!), ma coverage 93.3%.. COME MAI COSI' BASSO?
					CHE PAROLA VIENE FALLITA PER PRIMA?? (E VEDERE DI SISTEMARE VISUALIZZAZIONE NOTFOUND E RARECHARS...) !!!!!!!!!!

	e fare qualche prova con Currier A/Currier B? è che dovrei scaricare il sofwtare ivtt o come si chiama... hmmm... BIO (voyniches.com) sono 246 chunks mi pare con LOOP-4 standard

-----------------------------------

30/11/2024: inizio v010 011

06/12/2024 chiudo la 010 011, dopo ver inserito la scomposizione in chunks e la categorizzazione dei chunks (bel lavoro!). Passo alla 010 12 con l'idea di inserire 
	il computo delle frequenze BEGIN-->chunk1-->chunk2 ... --->END, per poi usare il tutto per generare pseudo-Voynichese casuale.

	Servirà un'altra tab... con report + output, come minimo....

	Non è che sia tutto perfetto nel software, in particolare quello che ho fatto per le grammatiche LOOP può causare problemi con grammatiche
		normali, anzi, ne causa:

				Fixare Zattera... cioè grammatiche non-loop, fa casini

                // Gestione specializzata per 'y' nelle LOOP grammars. Evita che p.es. chedy venga suddivisa come ched (nel primo chunk) + 'y' (nel secondo chunk),
                //   senza nemmeno passare dalla 'y' nella TAIL, e che quindi venga conteggiata come 2 chunks (mentre mi sembra più logico sia 1 chunk solo)
				// (RICORDARE, DOCUMENTAZIONE...)

				gestione specializzata 'q' nella head commentata, col che il programa crassha se si prova ad usarla (vari commenti in giro)!!!!!!

		Ma vabbè


			Manca:

				- intestazioni colonne nelle textBoxes della pagina Voynich
				- coverage C1 (calculatro sui word tokens invece che i word types)
				- distribuzione word types vs. # chunks
				- distribuzione chunk types (nella tab WrVoynich sarebbe semplice credo)

				manca gestione thread... yiikes


  ricordare i flags in Form1 show_rarechars, show_only_not_found_words!!!! da mettere in checkboxes prima o poi.....

----------------------

Inizio versione specializzata di TextAnalyzer 010 016 per parsing in word chunks del Voynich: 25/11/2024 MATTINA


- 30/11/2024: inserita pagina parsing (tab VOYNICH), che funziona piuttosto bene :) Ebbravo Mauro. Chiudo la versione per non correre rischi e passo alla 010 011,
	dove devo rendere piuù maneggevole la scelta delle grammatiche (poi vedrò se inserire altre) cose. Hmmm... e anche i flags di visualizzazione che adesso sono
	da ricompilare... sarebbe proprio da avere delle checxkboxes....



-----------------------------------------------------------------------------

ISTRUZIONI PER LA CONVERSIONE DEI FILES IN .TXT IN FORMATO UFT-8 - NON SPOSTARE DA QUA, IN TESTA AL README

- Se possibile partire da files nativi UTF8, e curati come uso della punteggiatura (ad averli...)
- Il Notepad di Windos è utilissimo perchè consente di aprire un .text con una codifica qualsiasi e poi di salvarlo con nome scegliendo il tipo di codifica:
		scegliere UFT8 o, ancora meglio, UFT8 con BOM (casellina appena a sinistra del pulsante 'salva')
- Word invece è una scocciatura perchè non permette di scegliere la codifica con cui salvare i files .txt. [Non so se salva sempre in UTF7 o se
		la codifica di salvataggio dipende dalla codifica del file .doc/.rtf originario]. Temo che la cosa migliore sia aprire il file .doc/.rtf, selezionare
		tutto e poi copiarlo in Notepad. Oppure salvare il .txt e poi, se causa problemi, aprirlo con NotePad e risalvarlo con la codifica giusta come detto sopra.
- Files .pdf: Acrobat sembra non abbia 'seleziona tutto', ma invece c'è, tramite lo shortcut Ctrl-A (che funziona anche in Word e Notepad)! Nota: su certi .pdf
		la copia Ctrl-C non funziona perchè l'autore ha bloccato la copia... stronzi. Nemmeno Word riesce ad aprirli. Con certi altri il Ctrl-C funziona, ma
		quello che si ottiene è un jumble di caratteri incomprensibili (anche aprendoli con Word). Hahah la cosa divertente è che ne ho trovato uno (Cesare Beccaria,
		dei delitti e delle pene) che così ad'occhio sembrava un cifrario a sostituzione semplice... l'ho salvato e comparato e, looll, è proprio un cifrario hahahaha
		(altri pdf usano encryptions un pò più robuste, superstronzi xD)

	Ricordare che un messaggio di errore avvisa quando, aprendo un file, si incontrano caratteri 'strani', che molto probabilmente sono dovuti ad un problema di codifica
	del .txt (il programma prova automaticamente UFT8 e UFT7, ma anche così si trovano files UFT7 in cui certi caratteri (p.es. quelli accentati) diventano caratteri
	di controllo o altre stranezze). Aprire il .txt col Notepad e salvarlo UFT8+BOM come detto sopra.

	Il controllo finale lo si fa comparando il file col corpus della sua lingua e controllando l'Additional Report che elenca le parole frequenti nel testo ma rare nel corpus,
	in modo da vedere se ci sono parole troncate o lunghe una sola lettera o simili! Con questo metodo si trappano anche problemi dovuti al testo in sè stesso e non alla sua
	codifica, per esempio ho trovato un.pdf che conteneva cose come 'Voi soli vo[i]rreste morire' o 'l<i> hanno tagliato', che non è ovviamente possibile trovare o
	correggere automaticamente,	ma che possono essere identificati con l'Additional report (se p.es. compaiono parole come 'vo' o 'rreste')

	E RICORDARE DI INCLUDERE I COMANDI DI PREPROCESSING quando si fa un'analisi, evita di perdere una cifra di tempo per capire come mai compaiono parole
	'strane' (che capita quando p.es. il testo ha usato un carattere non standard per l'apostrofo (tipicamente '’') che viene corretto dai comandi Regex di pre-processing)!!!!


 ISTRUZIONI PER L'USO DELLE OPZIONI DI PRE-PROCESSING

	In generale NON vanno modificate rispetto ai valori di default, questo perchè le opzioni usate influiscono sulle statistiche e potrebbero verificarsi dei problemi
		comparando, per esempio, files generati con opzioni di preprocessing diverse. Dato che l'idea di base di TextAnalyzer è di poter processare files indipendentemente
		dalla lingua in cui sono stati scritti è molto meglio usare sempre le stesse opzioni, anche se per casi particolari è pur sempre possibile modificarle.

			- Se si cerca di comparare files generati con opzioni diverse viene dato un warning. E' solo un warning perchè, dopotutto, non è detto che opzioni di pre-processing
				diverse	per caratteri quali dash o apostrofi spostino le statistiche talmente tanto da alterare i risultati delle comparazioni.
		
		Le opzioni migliori sono:

			Dash '-': discard words. Inutile stare lì a farsi le seghe mentali... unire le parole separate da dashes è carino, ma causa la comparsa di parole
										assurde quando l'autore del testo ha usato il dash enfaticamente in questo modo: scarpe-antisdrucciolo-di-cuoio-del-nord-dakota
										(esempio reale...), che creerebbe la parola scarpeantisdrucciolodicuoiodelnorddakota da 38 lettere... (in effetti era ancora più
										lunga... 54 lettere totali, ma non me la ricordo tutta xD). Anche separare le parole coi dashes è carino, ma causa la comparsa
										di parole strane, per esempio i-nu-til-men-te (enfatico) crea i, nu, til, men e te. Eliminarle senza pietà, che tanto sono poche!

			Apostrofo ''': l'opzione 'considera l'apostrofo come un carattere normale' è la migliore, per quanto abbia i suoi difetti:

					Hawaiano: perfetto, dato che l'apostrofo è davvero un carattere (stop glottale)
					Inglese: recupera "don't", "i'm", "won't" come parole, il che è bene perchè sono molto comuni. Purtroppo recupera come parole
							   anche i genitivi sassoni come "john's" e "mary's": bisogna farsene una ragione.
					Olandese: credo che vada bene sempre, recupera come parole "'t" e "s'hertogenbosch"
					Italiano: ortografia rognosissima... vengono recuperate come parole "l'amore", "dell'evento", "un'oca" etc., il che non solo allunga
								inutilmente il vocabolario, ma toglie anche le occorrenze apostrofate di "amore, "evento", "oca" etc. dalle statistiche,
								che è anche peggio. Ma dopo averci pensato su bene, ne ho dedotto che non c'è niente da fare:
									1) usare sempre le stesse opzioni per tutte le lingue è TROPPO importante (per capire provare a rispondere alla domanda: che
										opzioni uso per una lingua sconosciuta?)
									2) le alternative sono:
											2a) Unire le parole: ne crea di altrettanto assurde, anzi peggio, che se l'apostrofo fosse stato mantenuto ("lamore", "dellevento", "unoca")
												e allunga il vocabolario e altera le statistiche allo stesso modo che mantenendo l'apostrofo. Inferiore alla 1) da tutti i punti di vista.
											2b) Eliminare le parole sarebbe non male: evita l'allungamento del vocabolario e non inserisce parole strane, ma altera le statistiche di
													"amore, "evento" e "oca" allo stesso modo dell'opzione 1).
											2b) Separare le parole sarebbe buona: ne crea di assurde ("l", "dell") ma allunga di poco il vocabolario e, soprattutto, non altera le statistiche
												di "amore, "evento" e "oca". Si potrebbe usare per un'analisi specializzata per l'Italiano, ma la considerazione 1) è troppo importante
												se si vuole avere una gestione davvero language-indipendent!

					PS.: temo che il Lombardo Orientale sarebbe anche peggio dell'Italiano...

	NOTA

		Ci sono delle piccolissime differenze inevitabili fra le frequenze (p.es. bigrammi) calcolate tramite analisi di testi multipli e tramite aggregazione
			di analisi singole, questo perchè concatenando tutti i files in un'unica stringa vengono generati dei bigrammi aggiungivi (p.es. SPAZIO-lettera)
			nel punto in cui due files vengono concatenati. Poco male, ma siccome può generare confusione ho inserito questa nota anche nelle istruzioni in testa
			al readme. Probabile anche che, finito il debug, limiterò ad un solo file l'analisi dei testi.



	OSSERVAZIONI

		OSSERVAZIONE: i testi troppo corti non funzionano molto bene con le Comparazioni (c'era da aspettarselo). Cosa vogla dire 'troppo corti' non so, ma direi fore anche lunghi meno di 15K characters
			(che non sono pochi...)

		OSSERVAZIONE: i bigrammi fanno un discreto lavoro per identificare l'autore di un testo quando si comparano i testi, ma non c'è da pretendere troppo
			(e occhio ai testi troppo corti come detto sopra) P.es. Alfieri Vittorio - Filippo è vicino a tutti gli altri Alfieri, eccettuato Della Tirannide
			che è un outlier. Argomento da approfondire.
	    
		OSSERVAZIONE: usando tutti i caratteri invece che limitandoli col rare_characters_cutoff le statistiche unblinded in-cluster dei bigrammi migliorano lol
			(quelle out-cluster non cambiano significativamente). Anche altre statistiche si comportano allo stesso modo mi pare ma non ho fatto un'analisi sistematica
			(ho poi visto che non accade sempre così, nemmeno coi bigrammi, p.es. i testi di Dante si allontanano togliedo il rare_charcters_cutoff).

		OSSERVAZIONE: la statistica vocabulary blind distingue Voynich biological dal completo (in tutte e 3 le trascrizioni), che e' un'eccezione perche' in generale
			vocabulary blind non distingue affatto fra le lingue. Vedere il grafico 2d comparando i 6 voynich bio/completo nelle tre trascrizioni,
			Fra lingue 'vere' la distanza max vocabulary blind (fra De Bello Gallico e Midway Revisited) è 0.0468, la minima (fra Deledda - Dopo il Divorzio e
			Salgari - La regina dei caraibi) è 0.00588. Invece la distanza di Voynich EVA Biologial da Voynich EVA Completo è 0.039 (simile a De Bello Gallico - Canterbury tales 0.042
			o De Bello Gallico - Divina Commedia Inferno 0.037). Invece le altre statistiche raggruppano in tre clusters diversi le tre trascrizioni del Voynich, anche se credo
			con distanze sempre	sopra al limite accettato per considerarle la stessa lingua(ma è da verificare!!!)

------------------------------------------------------------------------------------------------------------------------------------------

TEXT ANALYZER 010 016

OBIETTIVI

 05 Giugno 2024

 		Più che altro avrei in programma dei ritocchi:

			- Controllo generale (con l'obiettivo di determinare cosa può essere effettivamente fatto, e cosa non, con la suddivisione in clusters).

			- Eliminazione delle 'vecchie' versioni della gestione clusters e XPlotClusters

			- Picturebox clusters scrollabile! OK 06/06/2024
			- Evento splitter sui clusters (e bisogna anche dare un nome agli splitters) OK 06/06/2024

			- Colorare il numero aggiunto ad un cluster pre-esistente con la sua distanza dall'elemento più vicino del cluster (che è la distanza minima, mentre il bordo
					esterno del cluster è la distanza massima. Spiegare in XPlotClusters!) INVECE HO INSERITO LA GESTIONE 'INTERATTIVA' COL MOUSEHOVER 11/06/2024
			- Indicare in qualche modo quali sono i due elementi che collegano fra di loro due clusters? Sarebbe esteremamente utile... ma come farlo?
					INSERITO NELLA GESTIONE 'INTERATTIVA' COL MOUSEHOVER 11/06/2024

			- Opzione per una scala 'assoluta' del clustering? E' che deve cambiare da statistica a statistica... RIMANDATO
			- Varie ed eventuali (titolo e scala colore XplotClusters) RIMANDATO


			XPlotClusters: sarebbe comodo un Ctrl-F nel grafico.. e un modo per copiarlo (dato che è enorme) RIMANDATO
			



SVILUPPO

	1) Inserita la pictureBox scrollabile sul grafico Clusters, ed ho inserito tutte le istruzioni per l'uso in XControls. En passant (per motivi sinceramente non chiari)
		rende non necessaria la gestione dell'evento splitterMoved (non piangiamoci sopra). 06/06/2024 mattina

	2) Adesso i parametri del grafico Clusters (larghezza righe, margini) vengono adattati al numero di testi da visualizzare 06/06/2024

	3) Introdotto il flag XPlotClusters.optimize_graph_area: se è false vengono bypassate le gestioni che piazzano cluster e numeri nella posizione migliore per minimizzare
		l'area del grafico. Si ottiene un grafico molto più largo (e basso), che non è graficamente carino come quello normale ma dove è molto più facile trovare i
		testi che si stanno cercando! 06/06/2024
		
	4) XPlotClusters interattivo

		06/06/2024 sera: inserita la gestione del MouseHover (presa da ArsMilitaris e annotata per bene anche in XControls). Per ora scrive solo le coordinate X,Y nella
			status window di Form1

		10/06/2024 riprendo dopo un doveroso week-end di pausa. Sul pomeriggio tardi tutta la base della gestione è a posto: determino il cluster in base alla posizione
			del mouse (ricorsivamente) e visualizzo un rudimento di dati in una pictureBox apposita [non ho proprio voglia di implementare una gestione che visualizzi
			i dati nella stessa bitmap del grafico].

		11/06/2024 mattina. Completo la gestione del MouseHover con la visualizzazione grafica dei dati nella pictureBox


	Sono a quasi due mesi serrati di software (la versione 010 010 è iniziata il 15 Aprile) e al momento sono piuttosto esaurito. Gli obiettivi della versione 016 sono stati
		raggiunti e anzi superati con la gestione 'interattiva' di XplotClusters e posso essere più che contento. Direi che TextAnalyzer 010 016 si può chiudere qua!

		si ma... eliminazione old??? controllo completo su un caso relativamente semplice?


CHIUSURA

	11 Giugno 2024

	



BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO

			stranezza delle distanze dei clusters: inspiegabile senza un disegno, metterla nella documentazione XPlotClusters!!!

			- Opzione per una scala 'assoluta' del clustering? E' che deve cambiare da statistica a statistica... RIMANDATO
			- Varie ed eventuali (titolo e scala colore XplotClusters) RIMANDATO


			XPlotClusters: sarebbe comodo un Ctrl-F nel grafico.. e un modo per copiarlo (dato che è enorme) RIMANDATO
			MOUSEHOVER CLUSTERS: scrivere il nome completo del file da qualche parte


	- GROVIGLI SOFTWARE

		Sono, ahimè, il primo problema

		1) Problemi con le threads in generale (altra bella menata). Qua seguono alcuni appunti:

			 Find best matches andrebbe messa in una thread per evitare un crash dovuto al timeout se si scandiscono moltissimi files (mai accaduto fin'ora però). Notare che anche
				la parte iniziale di load_and_analyze (dove vengono caricati i files, prima di lanciare la text_analyzer_main) potrebbe avere lo stesso problema. Inoltre
				anche la gestione di FormCompare andrebbe messa in una thread, adesso se si cerca di comparare moltissimi files (~300) si va in timeout (o il programma si blocca, se non si
				usa il debugger). Mass analyze sembra non abbia problemi anche se non è in una thread, forse sono le scritture dei files che salvano dal timeout? Mass aggregate 
				non è in una thread, non l'ho mai vista andare in timeout (con 309 files) ma... Noatre che sulle funzioni che non sono state inserite in una thread, ovviamente, 
				non viene lanciata la disable_controls_while_threads_are_running. Menù e pulsante rstano abilitati ma paer non ci si possa accedere. Boh... interessqante comunque.
				
				Sempre parlando di threads...:

					In mass_analyze c'è un error.Display_and_Clear che TEMO vada lanciato con un Invoke o provocherà un'eccezione.
					
					Ho modificato xstandards.mdfile.save, dove ho dovuto modificare due accessi	alla main_status_window aggiungendo gli Invokes
						(mentre un accesso con newline_to_main_status windows in una gestione di errore non è stato modificato, e anche qua c'è un
						error.Display_and_Clear). Ma... MI SONO ACCORTO DOPO che esiste già un delegate per il .save... il che vuol dire che avrei dovuto
						lanciare il .save con un Invoke, senza modificare la routine standard. 

		2) Per quanto abbia separato decentemente calcoli da visualizzazioni ci sono ancora dei casini.
				
				In FormCompare, contrariamente a tutte le altre functions di questo genere, display_report e display_additional_report non hanno una variante calculate_ e una display_
				avevo impostato male la cosa fin dall'inizio e la gestione del calcolo è troppo inframmischiata a quella della visualizzazione perchè valga la pena di sistemarla adesso

				In FromCompare bisognerebbe eseguire il doppio calcolo visualizzazione senza clustering/visualizzazione con clustering in calculate_2d_data/calculate_1d_data,
				 dadove andrebbe tolto ilo test checkBox_Compare_show_clusters_CheckedChanged (e testando la checkbox solo in visualizzazione ovviamente). E' CHE E' UNA PALLA....
				 SONO UNA CIFRA	DI VARIABILI DA AGGIUNGERE E DI CASI NELLE ROUTINE DI VISUALIZZAZIONE.... E IL TEMPO DI ELABORAZIONE COMUNQUE E' ABBASTANZA VELOCE, MAH...

				NOTARE: in FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
				assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpagliate quà e là. Non l'ho sistemato in Form1perchè c'è il rischio di
				infilarsi in qualche problema di Invoke, MA ME NE DEVO RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
				sono scritti meglio, USARE QUELLI PER IL TEMPLATE!! NOTA: FormClustering E' PIU' SEMPLCIE DI FormCompare E LI' CALCULATE_DATA/DISPLAY_DATA SONO SEPARATI PER
				BENE, RIFERIRSI A FormClustering PER QUESTA PARTE

		3) Per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
			'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
			MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
			I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE. AHHHH E I NOMI GIUSTI SAREBBERO EV_float, EV_long, EV_float_2d etc.!!!
			Inoltre ho tante di quelle varianti delle EVconvert() che non è facile starci dietro......

				IN GENERALE, GLI EVALUE SERVONO +- A QUALSIASI SOFTWARE.. PENSARCI ANCHE PER IL TEMPLATE

		4) Ho quintali di codice duplicato un po' dappertutto (in particolare nelle visualizzazioni, ma non solo)

		5) Bugs:

			Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
			il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
			FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

			Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
			finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
			semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
			chiusa se si lancia una nuova analisi o un nuovo compare...  IN GENERALE, IL DISCORSO DI COSA DISABILITARE E QUANDO ANDREBBE RIVISTO COMPLETAMENTE

			Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.

		6) Bug in XPlotClusters, che però non può mai verificarsi per come calcolo FormClustering.clustring results: sicuramente c'è la possibilità di sovrapposizioni di
			bounding_boxes in XPlotCluster.find_best_position e find_best_placement in casi sfigati. Vedere Documentazione XPlotClusters.docx, nella directory /bin/debug,
			dove spiego la cosa con un disegno.

		7) Debito tecnico. In XPlotClusters find_numbers_positions cercare // QUA BISOGNEREBBE TOGLIERE L'AVAILABLE_POINT UTILIZZATO, ED AGGIUNGERE  I DUE POINTS SUCCESSIVI,
				QUESTO PER UNIFORMITA' COL RESTO DELLE GESTIONI    //    E PER EVENTUALI ESPANSIONI FUTURE
		   
	   

	- NUOVE FUNZIONALITA'


		- Eliminare la possibilità di analizzare più di un file di testo alla volta ha risolto un bel problema [Mi consente di settare le variabili loaded_files_list.file_charset_size e .file_max_word_length sui valori dell'ultimo
			(e, adesso, unico) file caricato, mentre prima, coi multifile, restavano a zero (solito problema di non poter sapere dove un file inizia o finisce)] ma ha il
			difetto di non poter più usare un file di comandi di preprocessing (che potrebbe essere comodo). Si potrebbe semplicemente alzare il limite a due
			files (spiegando almeno nel manuale che il preprocessing deve essere il primo file (in ordine alfabetico, hmm testare che il nome inizi con '!'??) e di
			non includere altro pena bug nel valore delle variabili di loaded_files_list).

		- Se si vuole trovare le parole inusuali in molti testi la cosa è molto scomoda, dato che ogni volta bisogna caricare il reference e poi scegliere un corpus con
			cui comparare. Sarebbe bello introdurre una funzione 'Compare with corpus...' che eviti di dover selezionare ogni volta lo stesso file. Serve una variabile per
			il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova nullo, che direi di salvare in config e di visualizzare in options.
			UTILE!

		- Funzioni 'Analyze and Check': come le Analyze, ma esegue anche una comparazione col corpus di riferimento e visualizza la finestra
			additional report, in modo che si possa verificare subito se si vedono parole troncate o strane che fanno supporre problemi di ortografia
			nel testo sorgente. Serve una variabile per il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova
			nullo quando si inizia un' Analysis and Check, che direi di salvare in config e di visualizzare in options. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

				Dopodichè potrei fare una "Mass analyze, check & save": similmente a Mass Analyze, per ogni file .txt lancia l'analisi, poi la comparazione
								col corpus di riferimento e visualizza la pagina parole inusuali. Poi servono due pulsanti: 'accept' salva il file .txanalysis con lo
								stesso nome del .txt, 'don't accept' modifica il nome su disco del .txt incriminato per revisione successiva. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

		- Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
			SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
			FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
			nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
			SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

		- Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
			PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO



	- MIGLIORIE

		- Form1.findAnalysisClusters non gestisce 'preprocessing options are different' 

		- Sarebbe bello, nel caso sia attiva l'opzione "discard words containing apostrophes", creare una lista della parole rimosse. Questo renderebbe più utilizzabile
				l'opzione (che forse si potrebbe usare di default), eliminando un bel po' di parole 'spurie' p.es. dal vocabolario Italiano.

		- Sarebbe bello un 'reverse colors' per i grafici 2D....

		- Rimozione delle abbreviazioni monoletterali: purtroppo non è facile rimuovere anche abbreviazioni come q.o.d. o S.P.Q.R., questo perchè il replace Regex che uso elimina
				tre chars in totale (il carattere non-letterale che precede la lettera, la lettera e il punto che la segue), col risultato che p.es. " q.o.d." diventa " o " dopo
                il primo passaggio (ne faccio due in preprocessing), e la 'o' non viene eliminata. Dovrei rimpiazzare solo due caratteri per l'abbreviazioni, cosi' " q.o.d." diventa
				"  o. " e al secondo passaggio verrebbe eliminata anche la "o.", ma non so come fare con Regex e sinceramente non ci voglio perdere tempo adesso.
					PS.: un abbreviazione non viene eliminata se è a inizio file. POTREBBE ANCHE ESERE CONSIDERATO UN BUG (BTW E' RIPORTATO NEL MANUALE)
	
		- Mass analyze and save: ad ogni file viene ancora aggiornata la trackbar delle tables_sizes (tutto il resto delle visualizzazioni è stato
				eliminato per velocizzare l'esecuzione)... ma dato che c'entrano i graphs_limits sistemarlo può essere rischioso (E ANCHE POCO UTILE)

		- Find best matches non genera dei 'bei' grafici 2d di riepilogo, ma questo perchè, trovando testi a distanza ravvicinata, la scala Z dei grafici è molto
			espansa ed evidenzia anche le minime differenze nei testi. Bisognerebbe poter aggiungere un'outlier... ma va visto bene come sceglierlo
			(p.es. che abbia una distanza del doppio? del testo più distante fra i 24 selezionati? (ci si può anche annotare la lista dei testi
			e poi fare un Compare manuale aggiungiendo anche un outlier... ma selezionare i 24+1 testi per il Compare è una menata più unica che rara).			

		- Nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		- Find best matches usa sempre il default di rare_characters_cutoff. Renderlo programmabile anche per find best matches non è semplicissimo perchè
				non esiste una textBox che lo può fare (nè ha senso aggiungerne una globale in Analisi per solo questa funzione). Chiedere il valore quando
				si lancia find best matches sarebbe una scocciatura quando lo si usa. Forse un'opzione sarebbe meglio. Anche max_output_results è fisso
				(viene dichiarato nella getsione di find best matches, evento Click su findBestMatchesInToolStripMenuItem). Anche qua un'opzione sarebbe credo l'unica
				soluzione sensata.

		- hmmm... se nessun cluster unblinded ma ce n'e' uno dubious sarebbe bello fare l'analisi unusual words su quello... MA OCIO PERCHE' HO RICHIAMATO
			   PER NOME I DATI DI BASE (_unblinded etc.) IN TANTI PUNTI MI SA....... E CI SARANNO PROBLEMI ANCHE CON L'IDENTIFICAZIONE DEL FILE
			   DA COMPARARE PROBABILMENTE... STARE ATTENTI...

		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica ACCESSORIO
			Come accessorio alla decrittazione  vedere se può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto...).
				Magari nel report? L'HO SCRITTO NEL MANUALE

		- Visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tutti i casi visualizzare solo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE MA TUTT'ALTRO CHE INDISPENSABILE
			

		- In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). Tabellina da alcune prove fatte:

								Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

					BESTIA DI MODIFICA CHE COMPLETEREBBE IL SOFTWARE, MA CHE HA COME PREREQUISITO LA SISTEMAZIONE DEI GROVIGLI, SENNO' E' UN SUICIDIO

					Ricordare che è utile anche per la funzione 'Clustering' (ma in questo caso il tipo di statistica andrà scelto al momento del lancio del
						clustering, per non impiegare eoni di tempo di elaborazione)


	- In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina PER ORA HO INSERITO L'AVVISO NEL MANUALE
					
	- Avviso nell'additional reporty vocabulary su cosa fare se si vedono parole strane (c'è il file di pre-processing per eliminare i caratteri fallati? se c'è ma non
		funziona è un problema di codifica: aprire il file e sostituire manualmente i caratteri fallati. se sono proprio fallati in origine nel file buttarlo via..) 
		IL MAX SAREBBE UN PULSANTE DI HELP LOCALE... MA ADESSO NEL MANUALE SPIEGO PER BENE TUTTO L'ARGOMENTO

	- Analisi/vocabolario: inserire un numero progressivo quando visualizzo le parole del vocabolario per le parole. NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA
			text_analyzer.analysis_results.vocabulary_words_distribution in un EValue_extended per aggiungere nell'element_additional il numero progressivo. la lista
			è lunga ci si può impiegare del tempo. BOH, FORSE E' UNA COSA CHE NON SERVE NEANCHE POI TANTO, E COMUNQUE ANDREBBE DOPO AVER SISTEMATO UN PO' DI GROVIGLI
			
	- Il vowel_set è una menata... e si allunga sempre più... non ci sarà una function 'di sistema' per capire se un carattere è una
			vocale o una consonante? mi pare di aver cercato e non era facile.... e poi è tanto lavoro per le pseudosillabe... anche se sono carine hehe	     

	- Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH... GRAN LAVORO MENOSO PER
		POCO RISULTATO MI SA (ALMENO AL MOMENTO)

	- Grafici 1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE

	- Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	- XGraphics1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	- XGraphicsd: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	- XGraphics2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	- XGraphics2d e 1d: eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...

	- Mancano un mucchio di pulsanti 'Select all', e inoltre c'è il famoso bacherozzolo del .Select: se si è già sulla textBox col cursore non funziona!

	- Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del tipo di grafico
		visualizzato (1d o 2d),	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMENTE IMPORTANTE
		NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS', E LA COSA PUO' CREARE
		CONFUSIONE Già che siamo in argomento: sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
		E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
		cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
			NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
			LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)	

	- Inserire trackbars + caselline numeriche per settatura limiti min e max della scala Z dei grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	- Nella pagina Analisi/Chars Distances potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter se voglio anche far vedere i grafici lineari,
	   ma chissenefrega, potrei continuare a visualizzare i garfici 2d ma esportare nella textBox i dati 1d per poterli poi copiare in Excel ). Per uniformità andrebbe fatto anche nella
	   previous/following.... E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUZIIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	   DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA... en passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	   sia il caso di aggiungerli causando infiniti problemi di compatibilità coi files .txalysis) . BOH... NON SO QUANTO SIA UTILE, VEDERE

	- Pulsante e casella di testo per settare il limite dei grafici 2d anche nella pagina Analisi usando il rare_characters_cutoff come si fa col Compare
		(e salvataggio del file .cfg del rare_character_cutoff). OVVIAMENTE MANTENERE ANCHE LA TRACKBAR E LA CASELLINA DI TESTO CHE CI SONO ADESSO, E' SOLO
		UNA FUNZIONE AGGIUNTIVA!!



SCARTATE:

	Sarebbe proprio bello avere anche il il Compare-Report 2d, oltre che l'1d. MA NO! SERVE A NADA, BASTA ANDARE A VEDERE LA PAGINA BIGRAMMI E LI' C'E' GIA' IL
				GRAFICO 2D DESIDERATO.. CON TUTTE LE LINGUE IN COMPARAZIONE RAFFRONTATE FRA LORO.

    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... EVITARE COME LA PESTE...
	 MODIFICARE LE OPZIONI FRA UN FILE E L'ALTRO PUO' CAUSARE CASINI CON LE STATISTICHE (C'E' UN AVVISO APPOSITO) Mancano anche le altre opzioni che avevo immaginato
	 (set author, set title...) ma non so se valga la pena di implementarle davvero. DIREI PROPRIO DI NO! EXTRAMENOSE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA ANCHE PERCHE' I CRITTOGRAMMI VENGONO GIA' RICONOSCIUTI BENE, DIREI PROPRIO DI LASCIAR PERDERE

	Migliorare il sort delle tabelle dati 2d di riepilogo in FormCompare (i grafici colorati rosso-giallo-verde) in modo da far vedere meglio i clusters. INUTILE NEANCHE PROVARCI,
		IL PROBLEMA DI FONDO E' CHE I CLUSTERS SONO SPESSO TROPPO COMPLICATI (CON OVERLAPS FRA DI LORO) PER POTER ESSERE RAPPRESENTATI IN DUE DIMENSIONI. SERVE UN APPROCCIO
		DIVERSO 8CHE TENTERO' CON LA FUNZIONE SPECIALIZZZATA DI CLUSTERING)
	
	Dato che le statistiche delle comparazioni dipendono dai graphs-limits, si potrebbe provare a modificare automaticamente i limiti per vedere se si ottengono matches migliori?
	 Mah, può essere un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. DIREI UN GROSSO LAVORO CON TANTI 
	 POSSIBILI PITFALLS (IN PRIMIS IL TEMPO DI ELABORAZIONE, SUL QUALE SI POTREBBE FARE POCO) PER VALORE AGGIUNTO FRA LO SCARSO E IL NULLO



------------------------------------------------------------------------------------------------------------------------------------------

TEXT ANALYZER 010 015

OBIETTIVI

 25 Maggio 2024

	L'obiettivo è cercare di introdurre la nuova funzione di Clustering, che mi consentirà anche (sempre che funzioni) di valutare effettivamente cosa TextAnalyzer
		può (e non può) fare riguardo alla determinazione di epoca/autore/argomento/genere... 


SVILUPPO

	1) Introdotta la routine di clustering (basata per ora sulla frequenza bigrammi), che devo dire funziona piuttosto bene! Ovviamente la struttura che
		viene trovata è piuttosto complessa, output solo testuale al momento. 26/05/2024

	2) Introdotta una pagina di scelta opzioni per la routine di clustering, comprensiva, oltre che dei graphs_limits, anche della ComboBox per scegliere
		che statistica usare. CON L'OCCASIONE INAUGURO LA CLASSE XCONTROLS, DOVE PER ORA C'E' SOLO LA GESTIONE DELLE COMBOBOXES, CHE ANDRA' MESSA NEL
		TEMPLATE! (ditemi se è poco) 27/05/2024 pomeriggio

	3) Aggiunta la gestione dei graphs_limit_2d 27/05/2024 pomeriggio

	4) Aggiunta la gestione completa del clustering su tutte le statistiche! 2D, 1D e 1D con uso dei graphs_limits_2d (specializzata per i monogrammi). E' stato anche facile,
		la struttura di FormClustering direi sia quella che mi è venuta meglio come separazione delle varie fasi (preparazione dati, calcoli, display) 27/05/2024 pomeriggio
		(manca qualche statistica secondaria: monogrammi senza spazi e pseudo-sillabe, ma aggiungerle adesso è facile)

		31/05/2024 sono in fase avanzata coi grafici dei clusters. Stavolta ho affrontato il problema come meritava... ho definito la struttura dati (cosa che non è stata
			affatto banale) in un documento Word (Documentazione XPlotClusters.docx, nella directory /bin/debug), ho definito la struttura software top-down con tante belle 
			subroutines, e alla fine ho ottenuto una mitica classe COMPLETAMENTE RICORSIVA (prima volta nella mia vita lol) che poi visualizzo con la prima routine ricorsiva della
			mia vita. Grande! Al momento mi manca ancora la gestione degli overlaps nella classe VisualClusters (al momento li calcolo ma non li scrivo da nessuna parte),
			e la visualizzazione è ancora solo testuale, ma è tutto previsto per poter disegnare un grafico vero e proprio (devo 'solo' risolvere il problema di determinare le
			bounding boxes dei gruppi e poi di piazzarle una vicina alle altre).

	5) 01/06/2024 Wow, la base dei grafici dei clusters funziona :). Tanti bei rettangoli colorati più i numeri :). Adesso devo verificare per bene, e manca anora lo scarto
					dei clusters ridondanti (e ovviamentre, gli overlaps fra cluster). Inoltre con la gestione del piazzamento dei rettangoli fatta come ora c'è la possibilità
					che dei rettangoli colorati vadano a sovrapporsi (in casi sfigati, inutile che cerchi di spiegare qua). Ma nel complesso è un mito!!!!


		  03/06/2025 primo pomeriggio. Dopo una Domenica di pausa sistemo una serie di problemi vari e rognosetti nella gestione del piazzamento delle bounding_box children
						e dei numeri

				    Poi inserisco, a livello di clustering_results, l'ordinamento per distanza dei clusters in ogni layer (avere i clusters ordinati cosi' mi tornerà
						utile dopo).

					Poi inserisco anche l'ordinamento dei numeri, che migliora la visualizzazione (in FormClustering.display_textual_data, e poi in
						XPlotClusters.create_new-cluster e .extend_cluster)
					
	6) Ho rifatto completamente la gestione dei clusters eliminando i layers!! Funziona direi egregiamente ed è anche un bel po' più semplice di prima. Non è così dettagliata,
		nel senso che non considero assolutamente le sovrapposizioni fra clusters: per esempio, prima se trovavo [4,5] come cluster, seguito poi da [5,6], che è
		'sovrapposto' a [4,5], li tenevo entrambi e alla fine sarebbero confluiti nel cluster [4,5,6] = [4,5] + [5,6]. Adesso invece nel trovare [5,6] passo direttamente
		al cluster [4,5,6] = 6 + [4,5]. In questo modo si perdono un po' di informazioni, ma si ottiene una visualizzazione dei dati facile da interpetare. L'ho finita
		prima di mezzogiorno del 04/06/2024 (è venuta davvero molto semplice! e abbstanza elegante).

			Notare che adesso clustering_results nasce già in forma ricorsiva (prima era una lista). Convert_clusters_to_graph è diventata anch'essa una routine
			ricorsiva, molto semplice e che alla fine richiama la get_colored_box che avevop sviluppato per la versione precedente.

			Notare che la get-colored_box è molto più potente di quello che in effetti serve, dato che adesso, per come genero clusters_result, ogni cluster può
			essere composto solo da due children, o da un child + un numero o da due numeri, mentre la get_colored_box supporta un numero qualsiasi di children
			e di numeri.
		
			Ho mantenuto anche la versione vecchia (tutte le routines e variabili nuove sono state chiamate xxxNew). Direi che adesso mi convenga chiudere la
			V015 e poi passare alla V016, dove direi di eliminare tutte le vecchie gestioni e di aggiungere qualche accessorio, in particolare una pictureBox scrollabile
			per il grafico dei clusters (il grafico dei 309 testi italiani è mooolto più grande dello schermo xD, vedo solo boxes una dentro l'altra e nessun numero (ma
			non dubito che tutto stia funzionando come deve anche coi 309 testi).

	7) Rimosso l'ordinamento dei VisualClusters figli per dimensione della bounding box: non era una brutta idea dal punto di vista grafico, ma poteva posporre clusters
			con distanza minore a clusters con distanza maggiore 05/07/2024

	8) Sistemati il readme, XPlotClusters.docx, aggiornati il Manuale.docx e la Presentazione.docx

					
		


CHIUSURA

	05/06/2024 Bene bene, prima di infilarmi in qualche guaio direi di chiudere qua la versione. RICORDARE che nella 010 015 ho sia la 'vecchia' gestione dei clusters
		(complicata, difficile da interpretare, ma completa nel senso che calcola gli overlaps dei vari clusters, anche se poi non vengono visualizzati) che quella 'nuova'
		(semplice, facile da interpretare, ma meno completa).


		Per la prossima versione avrei in programma:

			- Controllo generale (con l'obiettivo di determinare cosa può essere effettivamente fatto, e cosa non, con la suddivisione in clusters).
			- Eliminazione delle 'vecchie' versioni della gestione cluysters e XPlotClusters
			- Picturebox clusters scrollabile!
			- Evento splitter sui clusters (e bisogna anche dare un nome agli splitters)
			- Colorare il numero aggiunto ad un cluster pre-esistente con la sua distanza dall'elemento più vicino del cluster (che è la distanza minima, mentre il bordo
					esterno del cluster è la distanza massima. Spiegare in XPlotClusters!)
			- Indicare in quelche modo quali sono i due elementi che collegano fra di loro due clusters?



BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO


	- GROVIGLI SOFTWARE

		Sono, ahimè, il primo problema

		1) Problemi con le threads in generale (altra bella menata). Qua seguono alcuni appunti:

			 Find best matches andrebbe messa in una thread per evitare un crash dovuto al timeout se si scandiscono moltissimi files (mai accaduto fin'ora però). Notare che anche
				la parte iniziale di load_and_analyze (dove vengono caricati i files, prima di lanciare la text_analyzer_main) potrebbe avere lo stesso problema. Inoltre
				anche la gestione di FormCompare andrebbe messa in una thread, adesso se si cerca di comparare moltissimi files (~300) si va in timeout (o il programma si blocca, se non si
				usa il debugger). Mass analyze sembra non abbia problemi anche se non è in una thread, forse sono le scritture dei files che salvano dal timeout? Mass aggregate 
				non è in una thread, non l'ho mai vista andare in timeout (con 309 files) ma... Noatre che sulle funzioni che non sono state inserite in una thread, ovviamente, 
				non viene lanciata la disable_controls_while_threads_are_running. Menù e pulsante rstano abilitati ma paer non ci si possa accedere. Boh... interessqante comunque.
				
				Sempre parlando di threads...:

					In mass_analyze c'è un error.Display_and_Clear che TEMO vada lanciato con un Invoke o provocherà un'eccezione.
					
					Ho modificato xstandards.mdfile.save, dove ho dovuto modificare due accessi	alla main_status_window aggiungendo gli Invokes
						(mentre un accesso con newline_to_main_status windows in una gestione di errore non è stato modificato, e anche qua c'è un
						error.Display_and_Clear). Ma... MI SONO ACCORTO DOPO che esiste già un delegate per il .save... il che vuol dire che avrei dovuto
						lanciare il .save con un Invoke, senza modificare la routine standard. 

		2) Per quanto abbia separato decentemente calcoli da visualizzazioni ci sono ancora dei casini.
				
				In FormCompare, contrariamente a tutte le altre functions di questo genere, display_report e display_additional_report non hanno una variante calculate_ e una display_
				avevo impostato male la cosa fin dall'inizio e la gestione del calcolo è troppo inframmischiata a quella della visualizzazione perchè valga la pena di sistemarla adesso

				In FromCompare bisognerebbe eseguire il doppio calcolo visualizzazione senza clustering/visualizzazione con clustering in calculate_2d_data/calculate_1d_data,
				 dadove andrebbe tolto ilo test checkBox_Compare_show_clusters_CheckedChanged (e testando la checkbox solo in visualizzazione ovviamente). E' CHE E' UNA PALLA....
				 SONO UNA CIFRA	DI VARIABILI DA AGGIUNGERE E DI CASI NELLE ROUTINE DI VISUALIZZAZIONE.... E IL TEMPO DI ELABORAZIONE COMUNQUE E' ABBASTANZA VELOCE, MAH...

				NOTARE: in FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
				assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpagliate quà e là. Non l'ho sistemato in Form1perchè c'è il rischio di
				infilarsi in qualche problema di Invoke, MA ME NE DEVO RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
				sono scritti meglio, USARE QUELLI PER IL TEMPLATE!! NOTA: FormClustering E' PIU' SEMPLCIE DI FormCompare E LI' CALCULATE_DATA/DISPLAY_DATA SONO SEPARATI PER
				BENE, RIFERIRSI A FormClustering PER QUESTA PARTE

		3) Per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
			'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
			MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
			I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE. AHHHH E I NOMI GIUSTI SAREBBERO EV_float, EV_long, EV_float_2d etc.!!!
			Inoltre ho tante di quelle varianti delle EVconvert() che non è facile starci dietro......

				IN GENERALE, GLI EVALUE SERVONO +- A QUALSIASI SOFTWARE.. PENSARCI ANCHE PER IL TEMPLATE

		4) Ho quintali di codice duplicato un po' dappertutto (in particolare nelle visualizzazioni, ma non solo)

		5) Bugs:

			Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
			il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
			FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

			Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
			finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
			semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
			chiusa se si lancia una nuova analisi o un nuovo compare...  IN GENERALE, IL DISCORSO DI COSA DISABILITARE E QUANDO ANDREBBE RIVISTO COMPLETAMENTE

			Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.

		6) Bug in XPlotClusters, che però non può mai verificarsi per come calcolo FormClustering.clustring results: sicuramente c'è la possibilità di sovrapposizioni di
			bounding_boxes in XPlotCluster.find_best_position e find_best_placement in casi sfigati. Vedere Documentazione XPlotClusters.docx, nella directory /bin/debug,
			dove spiego la cosa con un disegno.

		7) Debito tecnico. In XPlotClusters find_numbers_positions cercare // QUA BISOGNEREBBE TOGLIERE L'AVAILABLE_POINT UTILIZZATO, ED AGGIUNGERE  I DUE POINTS SUCCESSIVI,
				QUESTO PER UNIFORMITA' COL RESTO DELLE GESTIONI    //    E PER EVENTUALI ESPANSIONI FUTURE
		   
	   

	- NUOVE FUNZIONALITA'


		- Eliminare la possibilità di analizzare più di un file di testo alla volta ha risolto un bel problema [Mi consente di settare le variabili loaded_files_list.file_charset_size e .file_max_word_length sui valori dell'ultimo
			(e, adesso, unico) file caricato, mentre prima, coi multifile, restavano a zero (solito problema di non poter sapere dove un file inizia o finisce)] ma ha il
			difetto di non poter più usare un file di comandi di preprocessing (che potrebbe essere comodo). Si potrebbe semplicemente alzare il limite a due
			files (spiegando almeno nel manuale che il preprocessing deve essere il primo file (in ordine alfabetico, hmm testare che il nome inizi con '!'??) e di
			non includere altro pena bug nel valore dekle variabili di loaded_files_list).

		- Se si vuole trovare le parole inusuali in molti testi la cosa è molto scomoda, dato che ogni volta bisogna caricare il reference e poi scegliere un corpus con
			cui comparare. Sarebbe bello introdurre una funzione 'Compare with corpus...' che eviti di dover selezionare ogni volta lo stesso file. Serve una variabile per
			il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova nullo, che direi di salvare in config e di visualizzare in options.
			UTILE!

		- Funzioni 'Analyze and Check': come le Analyze, ma esegue anche una comparazione col corpus di riferimento e visualizza la finestra
			additional report, in modo che si possa verificare subito se si vedono parole troncate o strane che fanno supporre problemi di ortografia
			nel testo sorgente. Serve una variabile per il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova
			nullo quando si inizia un' Analysis and Check, che direi di salvare in config e di visualizzare in options. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

				Dopodichè potrei fare una "Mass analyze, check & save": similmente a Mass Analyze, per ogni file .txt lancia l'analisi, poi la comparazione
								col corpus di riferimento e visualizza la pagina parole inusuali. Poi servono due pulsanti: 'accept' salva il file .txanalysis con lo
								stesso nome del .txt, 'don't accept' modifica il nome su disco del .txt incriminato per revisione successiva. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

		- Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
			SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
			FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
			nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
			SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

		- Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
			PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO



	- MIGLIORIE

		- Form1.findAnalysisClusters non gestisce 'preprocessing options are different' 

		- Sarebbe bello, nel caso sia attiva l'opzione "discard words containing apostrophes", creare una lista della parole rimosse. Questo renderebbe più utilizzabile
				l'opzione (che forse si potrebbe usare di default), eliminando un bel po' di parole 'spurie' p.es. dal vocabolario Italiano.

		- Sarebbe bello un 'reverse colors' per i grafici 2D....

		- Rimozione delle abbreviazioni monoletterali: purtroppo non è facile rimuovere anche abbreviazioni come q.o.d. o S.P.Q.R., questo perchè il replace Regex che uso elimina
				tre chars in totale (il carattere non-letterale che precede la lettera, la lettera e il punto che la segue), col risultato che p.es. " q.o.d." diventa " o " dopo
                il primo passaggio (ne faccio due in preprocessing), e la 'o' non viene eliminata. Dovrei rimpiazzare solo due caratteri per l'abbreviazioni, cosi' " q.o.d." diventa
				"  o. " e al secondo passaggio verrebbe eliminata anche la "o.", ma non so come fare con Regex e sinceramente non ci voglio perdere tempo adesso.
					PS.: un abbreviazione non viene eliminata se è a inizio file. POTREBBE ANCHE ESERE CONSIDERATO UN BUG (BTW E' RIPORTATO NEL MANUALE)
	
		- Mass analyze and save: ad ogni file viene ancora aggiornata la trackbar delle tables_sizes (tutto il resto delle visualizzazioni è stato
				eliminato per velocizzare l'esecuzione)... ma dato che c'entrano i graphs_limits sistemarlo può essere rischioso (E ANCHE POCO UTILE)

		- Find best matches non genera dei 'bei' grafici 2d di riepilogo, ma questo perchè, trovando testi a distanza ravvicinata, la scala Z dei grafici è molto
			espansa ed evidenzia anche le minime differenze nei testi. Bisognerebbe poter aggiungere un'outlier... ma va visto bene come sceglierlo
			(p.es. che abbia una distanza del doppio? del testo più distante fra i 24 selezionati? (ci si può anche annotare la lista dei testi
			e poi fare un Compare manuale aggiungiendo anche un outlier... ma selezionare i 24+1 testi per il Compare è una menata più unica che rara).			

		- Nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		- Find best matches usa sempre il default di rare_characters_cutoff. Renderlo programmabile anche per find best matches non è semplicissimo perchè
				non esiste una textBox che lo può fare (nè ha senso aggiungerne una globale in Analisi per solo questa funzione). Chiedere il valore quando
				si lancia find best matches sarebbe una scocciatura quando lo si usa. Forse un'opzione sarebbe meglio. Anche max_output_results è fisso
				(viene dichiarato nella getsione di find best matches, evento Click su findBestMatchesInToolStripMenuItem). Anche qua un'opzione sarebbe credo l'unica
				soluzione sensata.

		- hmmm... se nessun cluster unblinded ma ce n'e' uno dubious sarebbe bello fare l'analisi unusual words su quello... MA OCIO PERCHE' HO RICHIAMATO
			   PER NOME I DATI DI BASE (_unblinded etc.) IN TANTI PUNTI MI SA....... E CI SARANNO PROBLEMI ANCHE CON L'IDENTIFICAZIONE DEL FILE
			   DA COMPARARE PROBABILMENTE... STARE ATTENTI...

		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica ACCESSORIO
			Come accessorio alla decrittazione  vedere se può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto...).
				Magari nel report? L'HO SCRITTO NEL MANUALE

		- Visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tutti i casi visualizzare solo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE MA TUTT'ALTRO CHE INDISPENSABILE
			

		- In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). Tabellina da alcune prove fatte:

								Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

					BESTIA DI MODIFICA CHE COMPLETEREBBE IL SOFTWARE, MA CHE HA COME PREREQUISITO LA SISTEMAZIONE DEI GROVIGLI, SENNO' E' UN SUICIDIO

					Ricordare che è utile anche per la funzione 'Clustering' (ma in questo caso il tipo di statistica andrà scelto al momento del lancio del
						clustering, per non impiegare eoni di tempo di elaborazione)


	- In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina PER ORA HO INSERITO L'AVVISO NEL MANUALE
					
	- Avviso nell'additional reporty vocabulary su cosa fare se si vedono parole strane (c'è il file di pre-processing per eliminare i caratteri fallati? se c'è ma non
		funziona è un problema di codifica: aprire il file e sostituire manualmente i caratteri fallati. se sono proprio fallati in origine nel file buttarlo via..) 
		IL MAX SAREBBE UN PULSANTE DI HELP LOCALE... MA ADESSO NEL MANUALE SPIEGO PER BENE TUTTO L'ARGOMENTO

	- Analisi/vocabolario: inserire un numero progressivo quando visualizzo le parole del vocabolario per le parole. NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA
			text_analyzer.analysis_results.vocabulary_words_distribution in un EValue_extended per aggiungere nell'element_additional il numero progressivo. la lista
			è lunga ci si può impiegare del tempo. BOH, FORSE E' UNA COSA CHE NON SERVE NEANCHE POI TANTO, E COMUNQUE ANDREBBE DOPO AVER SISTEMATO UN PO' DI GROVIGLI
			
	- Il vowel_set è una menata... e si allunga sempre più... non ci sarà una function 'di sistema' per capire se un carattere è una
			vocale o una consonante? mi pare di aver cercato e non era facile.... e poi è tanto lavoro per le pseudosillabe... anche se sono carine hehe	     

	- Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH... GRAN LAVORO MENOSO PER
		POCO RISULTATO MI SA (ALMENO AL MOMENTO)

	- Grafici 1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE

	- Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	- XGraphics1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	- XGraphicsd: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	- XGraphics2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	- XGraphics2d e 1d: eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...

	- Mancano un mucchio di pulsanti 'Select all', e inoltre c'è il famoso bacherozzolo del .Select: se si è già sulla textBox col cursore non funziona!

	- Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del tipo di grafico
		visualizzato (1d o 2d),	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMENTE IMPORTANTE
		NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS', E LA COSA PUO' CREARE
		CONFUSIONE Già che siamo in argomento: sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
		E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
		cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
			NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
			LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)	

	- Sempre in EvoTemplate c'erano le pictureBox scrollabili: non mi ricordo proprio come le usassi ma direi che possano tornare utili, vedere! male che vada si potrebbe
		anche infilarle nel prossimo Template SFIZIO

	- Inserire trackbars + caselline numeriche per settatura limiti min e max della scala Z dei grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	- Nella pagina Analisi/Chars Distances potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter se voglio anche far vedere i grafici lineari,
	   ma chissenefrega, potrei continuare a visualizzare i garfici 2d ma esportare nella textBox i dati 1d per poterli poi copiare in Excel ). Per uniformità andrebbe fatto anche nella
	   previous/following.... E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUZIIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	   DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA... en passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	   sia il caso di aggiungerli causando infiniti problemi di compatibilità coi files .txalysis) . BOH... NON SO QUANTO SIA UTILE, VEDERE

	- Pulsante e casella di testo per settare il limite dei grafici 2d anche nella pagina Analisi usando il rare_characters_cutoff come si fa col Compare
		(e salvataggio del file .cfg del rare_character_cutoff). OVVIAMENTE MANTENERE ANCHE LA TRACKBAR E LA CASELLINA DI TESTO CHE CI SONO ADESSO, E' SOLO
		UNA FUNZIONE AGGIUNTIVA!!



SCARTATE:

	Sarebbe proprio bello avere anche il il Compare-Report 2d, oltre che l'1d. MA NO! SERVE A NADA, BASTA ANDARE A VEDERE LA PAGINA BIGRAMMI E LI' C'E' GIA' IL
				GRAFICO 2D DESIDERATO.. CON TUTTE LE LINGUE IN COMPARAZIONE RAFFRONTATE FRA LORO.

    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... EVITARE COME LA PESTE...
	 MODIFICARE LE OPZIONI FRA UN FILE E L'ALTRO PUO' CAUSARE CASINI CON LE STATISTICHE (C'E' UN AVVISO APPOSITO) Mancano anche le altre opzioni che avevo immaginato
	 (set author, set title...) ma non so se valga la pena di implementarle davvero. DIREI PROPRIO DI NO! EXTRAMENOSE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA ANCHE PERCHE' I CRITTOGRAMMI VENGONO GIA' RICONOSCIUTI BENE, DIREI PROPRIO DI LASCIAR PERDERE

	Migliorare il sort delle tabelle dati 2d di riepilogo in FormCompare (i grafici colorati rosso-giallo-verde) in modo da far vedere meglio i clusters. INUTILE NEANCHE PROVARCI,
		IL PROBLEMA DI FONDO E' CHE I CLUSTERS SONO SPESSO TROPPO COMPLICATI (CON OVERLAPS FRA DI LORO) PER POTER ESSERE RAPPRESENTATI IN DUE DIMENSIONI. SERVE UN APPROCCIO
		DIVERSO 8CHE TENTERO' CON LA FUNZIONE SPECIALIZZZATA DI CLUSTERING)
	
	Dato che le statistiche delle comparazioni dipendono dai graphs-limits, si potrebbe provare a modificare automaticamente i limiti per vedere se si ottengono matches migliori?
	 Mah, può essere un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. DIREI UN GROSSO LAVORO CON TANTI 
	 POSSIBILI PITFALLS (IN PRIMIS IL TEMPO DI ELABORAZIONE, SUL QUALE SI POTREBBE FARE POCO) PER VALORE AGGIUNTO FRA LO SCARSO E IL NULLO


------------------------------------------------------------------------------------------------------------------------------------------

TEXT ANALYZER 010 014

OBIETTIVI

 15 Maggio 2024

	Per la versione 010 014 programmo l'esecuzione solo di alcune modifiche, di cui la prima è indispensabile mentre le altre sono delle migliorie.

		Punto 1) dei grovigli software, ma senza sistemare effettivamente i grovigli... leggere che c'è scritto tutto. E include l' "Aggregate", che va fatto per prima
			cosa in modo da verificare che funzioni, confrontandolo con la stessa analisi ma multifile (dopodichè disabilitare il multifile, semplicemente togliendolo dalla OpenFileDialog)
			OKAY 16/05/2024

		Se possibile: sort delle tabelle 2d di riepilogo (vedi Migliorie), è che è un algoritmo complicato e da studiare. OKAY 16/05/2024 (per quanto l'algoritmo potrebbe
			essere migliorato, ma adesso ho altri programmi poer questo aspetto)

		Funzione per il Compare sempre con lo stesso riferimento, per facilitare la ricerca di 'unusual words' (è fra 'nuove funzionalità') [RIMANDATA]

		Outlier in find best matches per migliorare i grafici 2d (è in Migliorie) [RIMANDATA]

		Sistemare almeno il problema dei congiungimenti fra punti inclinati anche quando dovrebbero essere dritti (perchè sono ancora istogrammi) nei grafici 1d. Vedere Migliorie,
			è una di quelle marcate XGraphs1d OKAY 25/05/2024

		Vedere se c'è qualcosa d'altro che valga la pena di fare (purchè sia semplice!) [NAH]

		Scrivere un manuale!! OKAY 23/05/2024



SVILUPPO

Iniziato il 15 Maggio 2024

	1) Nella versione 013 si finisce in un 'out of memory error' cercando di analizzare ~250 files di testo contemporaneamente. Il problema di fondo è l'impostazione che usato
			per semplificare lo sviluppo: riunire tutti i testi in un gigantesca stringa prima di processarli. Per superare questo problema senza dover ribaltare tutta la
			gestione ho inserito la funzione Mass Aggregate Analysis, che aggrega i files .txalysis (in numero sperabilmente illimitato) in un unico corpus, evitando di
			doverlo fare con l'analisi dei files di testo. Inserire la gestione è stato non eccessivamente difficile anche se menoso, e poi ho dovuto correre dietro ad
			una rogna di dati che cambiavano fra l'analisi di files di testo multipli e la stessa analisi ma fatta con Mass Aggregate. Alla fine è saltato fuori che
			le distribuzioni derivate dal vocabolario (words_length_vocabulary e le pesudosillabe) NON sono aggregabili. Capito quello non c'è voluto molto a sistemare
			il problema, ma non è stato facile. 15/05/2024 sera.

			La Analyze text files(s) continuerà ad andare in errore se si cerca di elaborare troppi files, ma avendo il Mass Aggregate non è più un problema. Vedrò
				poi di aggiungere un warning se si cerca di elaborare più di un tot di files.

			Nell'inserire il mass Aggregate ho anche razionalizzato un po' le routines di calcolo in TextCharStats, VocabularyStats e SyllablesStats.

			Ho inserito anche l'avviso 'opzioni di preprocessing divers', ma devo confessare che non l'ho provato.

			Nella loaded_text_files_list c'è un campo per la 'raw length' dei files, che in Analisi multifile di testi è per forza la lunghezza del file raw (non ho modo di
				sapere dove ogni file inizia e finisce all'interno della stringa in cui venono caricati tutti i testi), mentre con Mass Aggregate è per forza la
				lunghezza del file cleaned. E' un problema del cavolo dato che l'unica cosa a cui serve la raw length è per poter controllare manualmente che percentuale
				di un corpus deriva da ogni singolo testo: la lunghezza 'cleaned' è la migliore per fare questo, ma non è che si sbagli di tanto nemmeno con la lunghezza 'raw'
				(l'unica cosa che conta è il rapporto fra la lunghezza e il totale). Più che altro la variazione dei numeri fra l'analisi e l'aggregate causa confusione: ho modificato
				la scritta nel report per evitarla.

			Ci sono delle piccolissime differenze inevitabili fra le frequenze (p.es. bigrammi) calcolate tramite analisi di testi multipli e tramite aggregazione
				di analisi singole, questo perchè concatenando tutti i files in un'unica stringa vengono generati dei bigrammi aggiungivi (p.es. SPAZIO-lettera)
				nel punto in cui due files vengono concatenati. Poco male, ma siccome può generare confusione ho inserito questa nota anche nelle istruzioni in testa
				al readme. Probabile anche che, finito il debug, limiterò ad un solo file l'analisi dei testi (limitandomi a mettere a false la proprietà multifile
				di OpenFileDialog ma tenendo tutta la gestione nel software).


	2) Nell'inserire l'aggregate è saltato fuori un bug nelle sillabe: una parola senza consonati non veniva inserita come sillaba. Sistemato 15/05/2024.

	3) Inserito lo scarto delle abbreviazioni ( composte da non-literal + char + '.' ). Comando Regex aggiunto in preprocess_Regex. Funziona anche per 
		abbreviazioni come a.C. o s.v., ma non riesce a rimuovere completamente q.o.d. o S.P.Q.R. (annotato fra le migliorie) 15/05/2024 sera, ritoccato 16/01/2024 pomeriggio.

	4) Aggiunta alla loaded_text_files_list anche la dimensione del set di caratteri e la lunghezza massima delle parole di ogni file, per facilitare il 'debugging'
		dei files di testo sorgenti. 16/05/2024

	5) Ho dato una sistemata alle visualizzazioni nella main status window (ho anche introdotto un flag 'verbose' su certe routines per questo scopo). E già che
		c'ero ho anche eliminato il lancio della display_data se si sta facendo una mass analysis, cosa che ovviamente ha velocizzato di brutto l'esecuzione :)
		[è restato ancora un aggiornamento della trackbar delle tables_sizes... ma sistemare quello è rischioso, lasciamo perdere, lo annoto] 16/05/2024

				Mass analysis di 253 testi: 100 secondi
				Mass aggregate di 253 testi: 24 secondi

		Rigenero tutti i files .txtalysis per tutti i testi che ho (Friulano e Xosha compresi :p), poi riprovo tutte le funzionalità e mi sembra che funzioni tutto
			as expected, bene :)

	6) Per evitare confusione limito ad 1 file la routine Analyze (semplicemente, non setto Multifile a true nella OpenFileDialog). Questo mi consente anche
		di settare le variabili loaded_files_list.file_charset_size e .file_max_word_length sui valori dell'ultimo (e, adesso, unico) file caricato, mentre
		prima, coi multifile,  restavano a zero (solito problema di non poter sapere dove un file inizia o finisce). 16/05/2024 primo pomeriggio.

	7) Nell'additional report unusual words adesso scrivo anche il # di occorrenze nel testo di ogni parola inusuale (bello). 16/05/2024.

			16/05/2024 mi costruisco un corpus italiano con 305 testi, 103.76M caratteri, 17.7M parole, non male :) Vocabolario 336.645 parole lol... sillabe 15K/22K

			17-18-19/05 mi costruisco un limitato corpus inglese con 10 testi, 17.6M caratteri, 3.2M parole. Purtroppo è molto sbilanciato da tre testi eneormemente
							lunghi: opere complete di Shakespeare, King James Bible e un testoi di farmacologia clinica xD

						mi costruisco un limitato corpus latino con 10 testi, 5.4M caratteri, 781K word. E in gran parte si tratta di testi di autori italiani tardo medievali
							scritti in Latino

						mi costruisco un limitato corpus greco koine coi testi del nuovo testamento, 819K caratteri, 138K parole. Ho dovuto lottare coi diacritici greci, che
							ho scoperto esistono in diverse varianti di code point U+xxxx (vedere qua: https://en.wikipedia.org/wiki/Greek_diacritics)


	8) Elimino finalmente il disgraziato problema del doppio nome (nome file e user_assigned_name). Uccido user_assigned_name senza pietà e baso tutto sul nome del file.
			Col che miglioro anche la struttura software di Form1 (ero stato costretto a duplicare calculate_all_distances_2d causa problema di accesso ai nomi, adesso
			richiamo quella di FormCompare) e di FormCompare (non è più necessario inizializzare una lista dei nomi apposita). Era una bella scocciatura, bene. 20/05/2024 mattina

	9) Già che ci sono, elimino dal Compare e da FindBestMatches la comparazione con sè stesso (se lo stesso file è stato selezionato). Era proprio brutto a vedersi. 20/05/2024

	10) Introdotti in FormCompare i grafici di riepilogo con la visualizzazione del clustering (c'è una checkbox globale per selezionarli)! Grande xD 20/05/2024

			- Ovviamente, non garantiscono più che le distanze salgano monotonicamente lungo la prima riga/colonna della tabella
			- Inserendo in una comparazione files corti come il DHR Italiano e Friulano il clustering non è perfetto. Cercare di spiegarlo a parole è vano,
				vedrò di inserirlo nel manuale con dei disegni. L'algoritmo di clustering potrebbe sicuramente essere migliorato per risolvere anche
				questo aspetto, solo che è un gran casino.


		20-22/05/2024: mi dedico alla scrittura del manuale (che è importante! ed è lungo...)


	11) Ho praticamente completato il Manuale (tranne gli esempi). Aggiungo anche il numero totale delle parole di un testo alla loaded_files_list (è molto comodo per poter
			vedere subito quali testi sono toppo corti per un'analisi efficace) 23/05/2024

	12) Correzione bug: la legenda dei grafici multiserie 1d scriveva tutti i nomi doppi dopo al primo. Era un problema in Form1, raddopiavo tutti i records
			di analysis_filename durante la compareAnalysisFiles 23/05/2024

	13) Ho modificato la numerazione dei grafici 2D di riepilogo in modo che i numeri siano sempre ordinati per distanze, anche se è stata scelta la visualizzazione "cluster"
			(altrimenti era facile fare una gran confusione). Nel farlo ho anche dovuto modificare la scrittura delle labels orizzontali di XGraphs2d, perchè mentre
			prima passavo solo una stringa col numero adesso passo il numero + il nome completo del file, per cui con la vecchia gestione non ci sarebbe mai stato spazio
			per scrivere le lables orizzontali. Come vantaggio aggiuntivo adesso si vedono i nomi dei files (troncati, se sono lunghi) anche nei grafici di riepilogo,
			e si vedono meglio le labels orizzontali nei grafici normali. 23/05/2024 ora di cena.
		
			24/05/2024 più che altro ho pensato ad un algoritmo di clustering, concepito per poter essere usato con molti files (tipo i 309 del corpus) italiano

	14) Aggiunto un warning se si cerca di lanciare un Compare su più di 100 files (tempi diventano lunghissimi, e oltre un certo limite si va in timeout, il che è un
		bug conosciuto (non c'è la thread in FormCompare). 25/05/2024

	15) Correzione a XGraphs1d: ho sistemato il problema del congiungimento dei punti quando x_expanded_scale è <= 1 (era un vero e proprio bug). Tuttavia coi grafici multiserie
			il risultato finale era piuttosto brutto, così in questo caso mantengo una lieve pendenza per le linee di congiungimento (1 pixel per lato, anche se 
			sarebbe stato meglio aggiungere un parametro, eh vabbè). 25/05/2024

	16) Eliminata la gestione degli eventi MouseLeave dalle caselle di testo. Introdurli non era stata una grande idea... 25/05/2024


		Direi sia ora di chiudere



CHIUSURA

	25/05/2024 pomeriggio

	Molto bene, ho dato una bella risistemata a vari aspetti del software, fra cui tutti quelli che erano considerati 'Indispensabili'. Bella anche la funzione di clustering
		nei grafici riepilogo 2D, e l'idea che ho per il futuro di inserire un vero e proprio algoritmo di clustering e l'analisi che ne è seguita per vedere
		la fattibilità (direi di si, anche se non è semplice, e richiederebbe una visualizzazione tutta sua e incasinata, due dimensioni sono insufficienti allo scopo...
		ho qualche idea anche qua ma non le ho ancora scritte).

		Inoltre ho scrito il Manuale, che è MOLTO importante soprattutto per il futuro!




BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO


	- GROVIGLI SOFTWARE

		Sono, ahimè, il primo problema

		2) Problemi con le threads in generale (altra bella menata). Qua seguono alcuni appunti:

			 Find best matches andrebbe messa in una thread per evitare un crash dovuto al timeout se si scandiscono moltissimi files (mai accaduto fin'ora però). Notare che anche
				la parte iniziale di load_and_analyze (dove vengono caricati i files, prima di lanciare la text_analyzer_main) potrebbe avere lo stesso problema. Inoltre
				anche la gestione di FormCompare andrebbe messa in una thread, adesso se si cerca di comparare moltissimi files (~300) si va in timeout (o il programma si blocca, se non si
				usa il debugger). Mass analyze sembra non abbia problemi anche se non è in una thread, forse sono le scritture dei files che salvano dal timeout? Mass aggregate 
				non è in una thread, non l'ho mai vista andare in timeout (con 309 files) ma... Noatre che sulle funzioni che non sono state inserite in una thread, ovviamente, 
				non viene lanciata la disable_controls_while_threads_are_running. Menù e pulsante rstano abilitati ma paer non ci si possa accedere. Boh... interessqante comunque.
				
				Sempre parlando di threads...:

					In mass_analyze c'è un error.Display_and_Clear che TEMO vada lanciato con un Invoke o provocherà un'eccezione.
					
					Ho modificato xstandards.mdfile.save, dove ho dovuto modificare due accessi	alla main_status_window aggiungendo gli Invokes
						(mentre un accesso con newline_to_main_status windows in una gestione di errore non è stato modificato, e anche qua c'è un
						error.Display_and_Clear). Ma... MI SONO ACCORTO DOPO che esiste già un delegate per il .save... il che vuol dire che avrei dovuto
						lanciare il .save con un Invoke, senza modificare la routine standard. 

		4) Per quanto abbia separato decentemente calcoli da visualizzazioni ci sono ancora dei casini.
				
				In FormCompare, contrariamente a tutte le altre functions di questo genere, display_report e display_additional_report non hanno una variante calculate_ e una display_
				avevo impostato male la cosa fin dall'inizio e la gestione del calcolo è troppo inframmischiata a quella della visualizzazione perchè valga la pena di sistemarla adesso

				In FromCompare bisognerebbe eseguire il doppio calcolo visualizzazione senza clustering/visualizzazione con clustering in calculate_2d_data/calculate_1d_data,
				 dadove andrebbe tolto ilo test checkBox_Compare_show_clusters_CheckedChanged (e testando la checkbox solo in visualizzazione ovviamente). E' CHE E' UNA PALLA....
				 SONO UNA CIFRA	DI VARIABILI DA AGGIUNGERE E DI CASI NELLE ROUTINE DI VISUALIZZAZIONE.... E IL TEMPO DI ELABORAZIONE COMUNQUE E' ABBASTANZA VELOCE, MAH...

				NOTARE: in FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
				assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpagliate quà e là. Non l'ho sistemato perchè c'è il rischio di
				infilarsi in qualche problema di Invoke, MA ME NE DEVO RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
				sono scritti meglio, USARE QUELLI PER IL TEMPLATE!!

		5) Per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
			'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
			MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
			I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE. AHHHH E I NOMI GIUSTI SAREBBERO EV_float, EV_long, EV_float_2d etc.!!!
			Inoltre ho tante di quelle varianti delle EVconvert() che non è facile starci dietro......

				IN GENERALE, GLI EVALUE SERVONO +- A QUALSIASI SOFTWARE.. PENSARCI ANCHE PER IL TEMPLATE

		6) Ho quintali di codice duplicato un po' dappertutto (in particolare nelle visualizzazioni, ma non solo)

		7) Bugs:

			Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
			il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
			FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

			Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
			finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
			semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
			chiusa se si lancia una nuova analisi o un nuovo compare...  IN GENERALE, IL DISCORSO DI COSA DISABILITARE E QUANDO ANDREBBE RIVISTO COMPLETAMENTE

			Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.


	- NUOVE FUNZIONALITA'

		- Funzione di clustering (separata da tutto il resto, nuova classe e nuova Form) che possa lavorare su centinaia di files (anche se l'elaborazione sarà molto lunga).
			Ho preparato un file .xls (Esperimento Clusters) nel quale ho determinato al 70% un possibile modo di procedere. Per ora ovviamente solo sulle frequenze bigrammi, ma
			in futuro dovrebbe potre funzionare anche sulle altre statiutiche (una alla volta mi sà o i tempi di calcolo diventeranno biblici) e senza bisogno di
			soglie specializzate. SAREBBE DAVVERO INTERESSANTE PERCHE' PERMETTE	DI STABILIRE EFFETTIVAMENTE QUALI SIANO LE POTENZIALITA' DELLE STATISTICHE NEL DISCIRMINARE
			FRA LORO AUTORI/EPOCHE/ARGOMENTI/GENERI ETC. ETC.

		- Eliminare la possibilità di analizzare più di un file di testo alla volta ha risolto un bel problema [Mi consente di settare le variabili loaded_files_list.file_charset_size e .file_max_word_length sui valori dell'ultimo
			(e, adesso, unico) file caricato, mentre prima, coi multifile, restavano a zero (solito problema di non poter sapere dove un file inizia o finisce)] ma ha il
			difetto di non poter più usare un file di comandi di preprocessing (che potrebbe essere comodo). Si potrebbe semplicemente alzare il limite a due
			files (spiegando almeno nel manuale che il preprocessing deve essere il primo file (in ordine alfabetico, hmm testare che il nome inizi con '!'??) e di
			non includere altro pena bug nel valore dekle variabili di loaded_files_list).

		- Se si vuole trovare le parole inusuali in molti testi la cosa è molto scomoda, dato che ogni volta bisogna caricare il reference e poi scegliere un corpus con
			cui comparare. Sarebbe bello introdurre una funzione 'Compare with corpus...' che eviti di dover selezionare ogni volta lo stesso file. Serve una variabile per
			il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova nullo, che direi di salvare in config e di visualizzare in options.
			UTILE!

		- Funzioni 'Analyze and Check': come le Analyze, ma esegue anche una comparazione col corpus di riferimento e visualizza la finestra
			additional report, in modo che si possa verificare subito se si vedono parole troncate o strane che fanno supporre problemi di ortografia
			nel testo sorgente. Serve una variabile per il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova
			nullo quando si inizia un' Analysis and Check, che direi di salvare in config e di visualizzare in options. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

				Dopodichè potrei fare una "Mass analyze, check & save": similmente a Mass Analyze, per ogni file .txt lancia l'analisi, poi la comparazione
								col corpus di riferimento e visualizza la pagina parole inusuali. Poi servono due pulsanti: 'accept' salva il file .txanalysis con lo
								stesso nome del .txt, 'don't accept' modifica il nome su disco del .txt incriminato per revisione successiva. ACCESSORIO, AL MOMENTO DIREI POCO UTILE

		- Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
			SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
			FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
			nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
			SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

		- Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
			PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO



	- MIGLIORIE

		- Sarebbe bello, nel caso sia attiva l'opzione "discard words containing apostrophes", creare una lista della parole rimosse. Questo renderebbe più utilizzabile
				l'opzione (che forse si potrebbe usare di default), eliminando un bel po' di parole 'spurie' p.es. dal vocabolario Italiano.

		- Sarebbe bello un 'reverse colors' per i grafici 2D....

		- Rimozione delle abbreviazioni monoletterali: purtroppo non è facile rimuovere anche abbreviazioni come q.o.d. o S.P.Q.R., questo perchè il replace Regex che uso elimina
				tre chars in totale (il carattere non-letterale che precede la lettera, la lettera e il punto che la segue), col risultato che p.es. " q.o.d." diventa " o " dopo
                il primo passaggio (ne faccio due in preprocessing), e la 'o' non viene eliminata. Dovrei rimpiazzare solo due caratteri per l'abbreviazioni, cosi' " q.o.d." diventa
				"  o. " e al secondo passaggio verrebbe eliminata anche la "o.", ma non so come fare con Regex e sinceramente non ci voglio perdere tempo adesso.
					PS.: un abbreviazione non viene eliminata se è a inizio file. POTREBBE ANCHE ESERE CONSIDERATO UN BUG (BTW E' RIPORTATO NEL MANUALE)
	
		- Mass analyze and save: ad ogni file viene ancora aggiornata la trackbar delle tables_sizes (tutto il resto delle visualizzazioni è stato
				eliminato per velocizzare l'esecuzione)... ma dato che c'entrano i graphs_limits sistemarlo può essere rischioso (E ANCHE POCO UTILE)

		- Find best matches non genera dei 'bei' grafici 2d di riepilogo, ma questo perchè, trovando testi a distanza ravvicinata, la scala Z dei grafici è molto
			espansa ed evidenzia anche le minime differenze nei testi. Bisognerebbe poter aggiungere un'outlier... ma va visto bene come sceglierlo
			(p.es. che abbia una distanza del doppio? del testo più distante fra i 24 selezionati? (ci si può anche annotare la lista dei testi
			e poi fare un Compare manuale aggiungiendo anche un outlier... ma selezionare i 24+1 testi per il Compare è una menata più unica che rara).			

		- Nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		- Find best matches usa sempre il default di rare_characters_cutoff. Renderlo programmabile anche per find best matches non è semplicissimo perchè
				non esiste una textBox che lo può fare (nè ha senso aggiungerne una globale in Analisi per solo questa funzione). Chiedere il valore quando
				si lancia find best matches sarebbe una scocciatura quando lo si usa. Forse un'opzione sarebbe meglio. Anche max_output_results è fisso
				(viene dichiarato nella getsione di find best matches, evento Click su findBestMatchesInToolStripMenuItem). Anche qua un'opzione sarebbe credo l'unica
				soluzione sensata.

		- hmmm... se nessun cluster unblinded ma ce n'e' uno dubious sarebbe bello fare l'analisi unusual words su quello... MA OCIO PERCHE' HO RICHIAMATO
			   PER NOME I DATI DI BASE (_unblinded etc.) IN TANTI PUNTI MI SA....... E CI SARANNO PROBLEMI ANCHE CON L'IDENTIFICAZIONE DEL FILE
			   DA COMPARARE PROBABILMENTE... STARE ATTENTI...

		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica ACCESSORIO
			Come accessorio alla decrittazione  vedere se può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto...).
				Magari nel report? L'HO SCRITTO NEL MANUALE

		- Visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tutti i casi visualizzare solo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE MA TUTT'ALTRO CHE INDISPENSABILE
			

		- In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). Tabellina da alcune prove fatte:

								Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

					BESTIA DI MODIFICA CHE COMPLETEREBBE IL SOFTWARE, MA CHE HA COME PREREQUISITO LA SISTEMAZIONE DEI GROVIGLI, SENNO' E' UN SUICIDIO

					Ricordare che è utile anche per la funzione 'Clustering' (ma in questo caso il tipo di statistica andrà scelto al momento del lancio del
						clustering, per non impiegare eoni di tempo di elaborazione)


	- In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina PER ORA HO INSERITO L'AVVISO NEL MANUALE
					
	- Avviso nell'additional reporty vocabulary su cosa fare se si vedono parole strane (c'è il file di pre-processing per eliminare i caratteri fallati? se c'è ma non
		funziona è un problema di codifica: aprire il file e sostituire manualmente i caratteri fallati. se sono proprio fallati in origine nel file buttarlo via..) 
		IL MAX SAREBBE UN PULSANTE DI HELP LOCALE... MA ADESSO NEL MANUALE SPIEGO PER BENE TUTTO L'ARGOMENTO

	- Analisi/vocabolario: inserire un numero progressivo quando visualizzo le parole del vocabolario per le parole. NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA
			text_analyzer.analysis_results.vocabulary_words_distribution in un EValue_extended per aggiungere nell'element_additional il numero progressivo. la lista
			è lunga ci si può impiegare del tempo. BOH, FORSE E' UNA COSA CHE NON SERVE NEANCHE POI TANTO, E COMUNQUE ANDREBBE DOPO AVER SISTEMATO UN PO' DI GROVIGLI
			
	- Il vowel_set è una menata... e si allunga sempre più... non ci sarà una function 'di sistema' per capire se un carattere è una
			vocale o una consonante? mi pare di aver cercato e non era facile.... e poi è tanto lavoro per le pseudosillabe... anche se sono carine hehe	     

	- Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH... GRAN LAVORO MENOSO PER
		POCO RISULTATO MI SA (ALMENO AL MOMENTO)

	- Grafici 1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE

	- Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	- XGraphics1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	- XGraphicsd: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	- XGraphics2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	- XGraphics2d e 1d: eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...

	- Mancano un mucchio di pulsanti 'Select all', e inoltre c'è il famoso bacherozzolo del .Select: se si è già sulla textBox col cursore non funziona!

	- Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del tipo di grafico
		visualizzato (1d o 2d),	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMENTE IMPORTANTE
		NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS', E LA COSA PUO' CREARE
		CONFUSIONE Già che siamo in argomento: sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
		E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
		cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
			NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
			LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)	

	- Sempre in EvoTemplate c'erano le pictureBox scrollabili: non mi ricordo proprio come le usassi ma direi che possano tornare utili, vedere! male che vada si potrebbe
		anche infilarle nel prossimo Template SFIZIO

	- Inserire trackbars + caselline numeriche per settatura limiti min e max della scala Z dei grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	- Nella pagina Analisi/Chars Distances potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter se voglio anche far vedere i grafici lineari,
	   ma chissenefrega, potrei continuare a visualizzare i garfici 2d ma esportare nella textBox i dati 1d per poterli poi copiare in Excel ). Per uniformità andrebbe fatto anche nella
	   previous/following.... E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUZIIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	   DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA... en passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	   sia il caso di aggiungerli causando infiniti problemi di compatibilità coi files .txalysis) . BOH... NON SO QUANTO SIA UTILE, VEDERE

	- Pulsante e casella di testo per settare il limite dei grafici 2d anche nella pagina Analisi usando il rare_characters_cutoff come si fa col Compare
		(e salvataggio del file .cfg del rare_character_cutoff). OVVIAMENTE MANTENERE ANCHE LA TRACKBAR E LA CASELLINA DI TESTO CHE CI SONO ADESSO, E' SOLO
		UNA FUNZIONE AGGIUNTIVA!!



SCARTATE:

	Sarebbe proprio bello avere anche il il Compare-Report 2d, oltre che l'1d. MA NO! SERVE A NADA, BASTA ANDARE A VEDERE LA PAGINA BIGRAMMI E LI' C'E' GIA' IL
				GRAFICO 2D DESIDERATO.. CON TUTTE LE LINGUE IN COMPARAZIONE RAFFRONTATE FRA LORO.

    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... EVITARE COME LA PESTE...
	 MODIFICARE LE OPZIONI FRA UN FILE E L'ALTRO PUO' CAUSARE CASINI CON LE STATISTICHE (C'E' UN AVVISO APPOSITO) Mancano anche le altre opzioni che avevo immaginato
	 (set author, set title...) ma non so se valga la pena di implementarle davvero. DIREI PROPRIO DI NO! EXTRAMENOSE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA ANCHE PERCHE' I CRITTOGRAMMI VENGONO GIA' RICONOSCIUTI BENE, DIREI PROPRIO DI LASCIAR PERDERE

	Migliorare il sort delle tabelle dati 2d di riepilogo in FormCompare (i grafici colorati rosso-giallo-verde) in modo da far vedere meglio i clusters. INUTILE NEANCHE PROVARCI,
		IL PROBLEMA DI FONDO E' CHE I CLUSTERS SONO SPESSO TROPPO COMPLICATI (CON OVERLAPS FRA DI LORO) PER POTER ESSERE RAPPRESENTATI IN DUE DIMENSIONI. SERVE UN APPROCCIO
		DIVERSO 8CHE TENTERO' CON LA FUNZIONE SPECIALIZZZATA DI CLUSTERING)
	
	Dato che le statistiche delle comparazioni dipendono dai graphs-limits, si potrebbe provare a modificare automaticamente i limiti per vedere se si ottengono matches migliori?
	 Mah, può essere un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. DIREI UN GROSSO LAVORO CON TANTI 
	 POSSIBILI PITFALLS (IN PRIMIS IL TEMPO DI ELABORAZIONE, SUL QUALE SI POTREBBE FARE POCO) PER VALORE AGGIUNTO FRA LO SCARSO E IL NULLO

-------------------------------------------------------------------------------

TEXT ANALYZER 010 013

OBIETTIVI/SVILUPPO

Iniziato il 10 Maggio 2024

Questa è la ToDo list da Text Analyzer 010 012, prioritizzata e as-built:


		1) Dato che con le modifiche che dovrò fare modifico la struttura dei files .txa, come primissima cosa modifico l'estensione a .txtalysis per non fare confusione
			coi files vecchi. En passanto ho anche modificato le informazioni di copyright del programma da Mauro Lanzini a Mauro L., privacy first! 10/05/2024

		2) Inserita una parola di esempio nel grafico della words_length_distribution_in_text. Non è stato banale perchè ho dovuto definire un nuovo tipo di dati (EValue_extended)
			e adesso ho tante di quelle varianti delle EVconvert() che non è facile starci dietro. En passant nel corpus italiano c'è una parola di 54 lettere che è
			d'avanguardiasuolaantisdrucciololaccioincuoiodeldakota xD, che direi venga dall'eliminazione dei dashes '-' con congiungimento delle parole
			(infatti: modificando l'opzione di gestione dei dashes a 'separate words' le parole lunghissime scompaiono, ora la più lunga è klausermanspitzwegensdorfentag...
			chissà in quale testo è xD). Dei dashes non me ne preoccupo adesso, ma quello che è importante è che l'aggiunta della parola di esempio aiuti nel debug dei testi. 10/05/2024

		3) Adesso calcolo anche la words_length_distribution_in_vocabulary durante l'analisi e la salvo in text_analyzer e nei files .txalysis (con parola di esempio
			 anche qua) 10/05/2024

		4) Non salvo più il loaded_text nel file .txanalysis (è sotto il controllo della stessa opzione che salva il raw_source_text). Era un bel
			problema perchè, oltre che allungare le dimensioni del file, poteva anche creare problemi di copyright. 10/05/2024

		5) Molto migliorata la gestione delle pseudo-sillabe: invece di mettere tutte le consonanti inter-nucleo come prefisso del secondo nucleo, adesso
			le divido in due gruppi e ne assegno metà come suffisso del primo nucleo e metà come suffisso del secondo (con priorità per il secondo nucleo
			se le consonanti sono in numero dispari). Ottengo delle sillabe molto più naturali, p.es. 'parte' viene sillabata par-te invece che pa-rte come
			accadeva prima. Chiaro che restano dei casi ingestibili, p.es. madre viene sillabata mad-re, ma per poter fare questo serve la scala di sonorità,
			e quindi molte più infromazioni su cosa i caratteri rappresentino effettivamente nel testo (altro esempio: 'vincitrice' viene sillabata vin-cit-ri-ce).
			E, ovviamente, i casi dovuti ad ortografie mal concepite, p.es. 'rovescio' viene sillabato ro-ves-cio (il programma non sa che -sci- è un suono unico)
			o 'analysis' sillabato a-nal-ysis (il programma non sa che, in questo caso, 'y' è una vocale). Comunque sia, ottimo risultato con algoritmo
			concettualmente semplice e che usa il minimo di informazioni possibile (solamente quali caratteri sono delle vocali). 10/05/2024 pomeriggio
		
		6) A seguito di un lungo e complicato ragionamento sulle opzioni di pre-processing (riportato poi nelle istruzioni in testa al readme, perchè è importante) 
			ho deciso di inserire subito le gestioni delle opzioni 'discard words containing dashes' e 'discard words containing apostrophes', questo perchè
			l'eliminazione delle parole contenenti dashes '-' mi sembra la migliore possibile da usare (e già che c'ero ho inserito anche la gestione per gli
			apostrofi). 10/05/2024 sera tardi. En passant ho deciso che tanto vale sillabare sempre anche le parole che contengono apostrofi (credeteci, è meglio così).
			Uh, finiti di sistemare i commenti è arrivata mezzanotte... giornata di modifiche strutturali menose, algoritmi rognosi (le pseudo-sillabe) e ragionamenti
			complessi (le opzioni) con modifiche rognose (le opzioni 'discard'). Speriamo che domani sia più divertente xD

		7) Inserito un'avviso se si cerca di comparare files creati con opzioni di preprocessing diverse. 11/05/2024 mattina

		8) Inserisco nel preprocessing anche la conversione dell'apostrofo non-standard '’' (che è usato spesso) in quello standard. 11/05/2024

		9) Notevole miglioramento dell'estrazione parole inusuali, che adesso è proprio mitico!! Uso tutte le parole del vocabolario target per la ricerca
			(ha allungato a qualche secondo il tempo di elaborazione con l'italiano, ma vabbè) e accetto anche le parole che vengono trovate, purchè la loro
			frequenza nel vocabolario reference sia amplificata di almeno amplification_threshold volte. 11/05/2024 mezzogiorno.
								
		10) Ho limitato la comparazione dei vocabolari ad un solo file .txanalysis per una serie di motivi:
		                1) Questa funzione è prevista per comparare un testo and un singolo .txalysis di riferimento (tipicamente, il corpus del linguaggio)
							  e avrebbe poco senso farla sull'intera lista dei comparandi                
						2) Limitandosi a comparare con un solo file, inoltre, si evitano un mucchio di menate software (come la necessità di usare una List<List>> invece
							che una List<> semplice per unusual_words_list, INOLTRE IL LOOP DIVENTA DIFFICILE DA SCRIVERE... CREDETEMI) e l'output nella text box mantiene
							una lunghezza ragionevole. 11/05/2024 primissimo pomeriggio.

						Inoltre, dato che c'è un casotto aggiuntivo (cercare ATTENZIONE!!! SE C'E' PIU' DI UN FILE NEL CLUSTER  in calculate_and_display_additional_report)
							che si verifica solo con le comparazioni multifile e che non vale la pena di sistemare, ho deciso di non eseguire del tutto la comparazione
							dei vocabolari in caso di compare multifile.

		11) Introdotte due caselline per poter programmare words_to_examine e amplification_threshold. 11/05/2024, bene, è un mito!

		12) Nel file di analisi ho aggiunto al nome dei files caricati anche la loro lunghezza in caratteri 11/05/2024 metà pomeriggio (giornata del cavolo come tempo a disposizione sigh)

		13) Inserita la funzione 'mass analyze and save analysis'. Non la cosa più semplice del mondo causa problemi di threading e di organizzazione delle routines,
				ma alla fine risulta proprio utile. Bene. 11/05/2024 18:30 

		14) Inserite delle scritte nella main_statuswindow man mano che procede il compare (ho provato ha comparare 90 files.. ci mette un po' di tempo e le scritte
				servivano). E facendolo scopro anche il bug che non faceva disabilitare l'evento TextChanged alla textBox_Compare_linear_size ...  toglievo l'handler
				alla trackBar, poi causa errore di nome ne aggiungevo uno in più alla textBox... Carognoso lol. 11/05/2024 19:45


		15)	Find best matches funzionante! 11/05/2024, quasi mezzanotte (richiede alcuni ritocchi che però non influiscono sui risultati).
		
			
		16)	12/05/2024: giornata dedicata a scrivere il documento Word di 'presentazione' di TextAnalyzer. Niente software oggi (a parte correzione di un bug di cui mi sono
				accorto scrivendo la persentazione... avavo scritto un _unblinded invece che un _blind, col risultato che un testo in spagnolo si infilava dentro 
				al cluster di Amore di Loredana codificato (mentre avrebbe dovuto essere fra i 'dubious')!

				scopro anche che nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		17)	13/05/2024: giornata dedicata a costruirmi un corpus Italiano adeguato. Ovviamente scopro anche delle rogne...

				Superati i ~200 files di testo analizzati contemporaneamente per creare un corpus slata fuori un 'out of memory error' sul  comando regex usato per
					contare gli spazi nel testo complessivo ( Regex.Matches(processed_text, "\\s").Count in TextPreprocessor ). Elimino il conteggio degli
					spazi fatto con Regex (lo calcolo in un altro modo in VocabularyStats, dopo che il testo è stato separato in parole, anche se non è il posto più logico
					in cui mettere il calcolo).  PERO' E' SPIA DI POSSIBILI PROBLEMI CON REGEX SU FILE MOLTO LUNGHI, IL CHE RIENTRA NEL DISCORSO GENERALE CHE CARICARE TUTTI
					IN FILES IN UN'UNICA GIGANTESCA STRINGA PER POI INIZIARE IL	PREPROCESSING NON E' CERTO IL MASSIMO...	HA ACCELERATO LO SVILUPPO DEL SOFTWARE MA ADESSO
					E' UN LIMITE: IL PREPROCESSING ANDREBBE PROPRIO FATTO FILE PER FILE... E' CHE E' UN CASINO

					EHH... ARRIVATI A ~250 FILES DI TESTO 'OUT MEMORY ERROR', QUESTA VOLTA IMPOSSIBILE DA SISTEMARE...  SIGH


		Okay, arrivati qua, col problema del memory error quando si superano i ~250 files direi sia ora di chiudere la versione, dato che oramai i grovigli software sono
			troppi per poter affrontare alrte modifiche o aggiunte senza rischiare, ed è meglio che non rischi.

			Comunque... ottimo risultato, il programma ha raggiunto completamente le aspettative che mi ero fatto!!




CHIUSURA

	15/05/2024

	Chiudo qua la versione 010 013.

	Per la versione 010 014 programm l'esecuzione solo di alcune modifiche, di cui la prima è indispensabile mentre le altre sono delle migliorie.

		Punto 1) dei grovigli software, ma senza sistemare effettivamente i grovigli... leggere che c'è scritto tutto. E include l' "Aggregate", che va fatto per prima
			cosa in modo da verificare che funzioni, confrontandolo con la stessa analisi ma multifile (dopodichè disabilitare il multifile, semplicemente togliendolo dalla OpenFileDialog)

		Funzione per il Compare sempre con lo stesso riferimento, per facilitare la ricerca di 'unusual words' (è fra 'nuove funzionalità')

		Outlier in find best matches per migliorare i grafici 2d (è in Migliorie)

		Sistemare almeno il problema dei congiungimenti fra punti inclinati anche quando dovrebbero essere dritti (perchè sono ancora istogrammi) nei grafici 1d. Vedere Migliorie,
			è una di quelle marcate XGraphs1d

		Vedere se c'è qualcosa d'altro che valga la pena di fare (purchè sia semplice!)

		Se possibile: sort delle tabelle 2d di riepilogo (vedi Migliorie), è che è un algoritmo complicato e da studiare.

		SCRIVERE UN MANUALE!!!! (INCLUDERE ANCHE GLI SVILUPPI FUTURI PREVISTI, LE OSSERVAZIONI E LE ISTRUZIONI IN TESTA AL README!! E SPIEGAZIONI
			SU COSA FARE SE SI TROVANO PAROLE TRONCHE/BALORDE NELL'ADDITIONAL REPORT).







BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO


	- GROVIGLI SOFTWARE

		Sono, ahimè, il primo problema

		1) Cercando di analizzare ~250 files di testo si finisce in un 'out of memory error'. Il problema di fondo è l'impostazione che usato per semplificare lo sviluppo:
			riunire tutti i testi in un gigantesca stringa prima di processarli. Invece bisogna caricare i testi e processarli uno per uno, ed ogni volta aggregare le statistiche di
			un testo coi precedenti. Purtroppo è una modifica tutt'altro che semplice. OK, MA VA FATTA PERCHE' L'IMPOSTAZIONE USATA ADESSO E' ASSURDA. IN ALTERNATIVA:
			LIMITARE Analyze AD UN SOLO TESTO E USARE MASS ANALYZE PER CREARE I FILES .txalysis, POI SCRIVERE LA FUNZIONE 'MASS AGGREGATE' PER AGGREGARE I .txalysis
			DIREI CHE SIA MOLTO PIU' SEMPLICE... E FORSE LO POSSO FARE SUBITO LOL... COL CHE POTREI ANCHE INTRODURRE LA MIGLIORIA 'ELIMINA ABBREVIAZIONI DI 1 LETTERA',
			E INOLTRE IL FILE DI PREPROCESSING NON ESISTEREBBE PIU' (I COMANDI ANDREBBERO INSERITI CASO PER CASO IN OGNI TESTO) E ANCHE QUESTO MI EVITA UN MUCCHIO DI MENATE...
			POTREI SOLO RISCHIARE ROGNE DI THREADING.... HMMMMM....

				RICORDARE L'AVVISO 'OPZIONI DIFFERENTI' CON LA FUNZIONE AGGREGATE!!
			
				E SE SISTEMASSI ANCHE IL PROBLEMA DELL'USER_NAME AVREI COMUNQUE A POSTO LE ROGNE PEGGIORI SENZA DOVER RIVEDERE TROPPO IL SOFTWARE... HMMMM.. E' UN'IDEA....

		2) Problemi con le threads in generale (altra bella menata). Qua seguono alcuni appunti:

			 Find best matches andrebbe messa in una thread per evitare un crash dovuto al timeout se si scandiscono moltissimi files. Notare che anche la parte iniziale
				di load_and_analyze (dove vengono caricati i files, prima di lanciare la text_analyzer_main) potrebbe avere lo stesso problema. Sempre parlando di threads...:

					In mass_analyze c'è un error.Display_and_Clear che TEMO vada lanciato con un Invoke o provocherà un'eccezione.
					
					Ho modificato xstandards.mdfile.save, dove ho dovuto modificare due accessi	alla main_status_window aggiungendo gli Invokes
						(mentre un accesso con newline_to_main_status windows in una gestione di errore non è stato modificato, e anche qua c'è un
						error.Display_and_Clear). Ma... MI SONO ACCORTO DOPO che esiste già un delegate per il .save... il che vuol dire che avrei dovuto
						lanciare il .save con un Invoke, senza modificare la routine standard.

		3) Orrenda confusione coi nomi dei files. Tutto è derivato dall'avere un campo separato per il nome (TextAnalyzerClass user_assigned_name), quando
			sarebbe stato molto meglio usare solo il nome del file. Questo non solo causa la sfigata possibilità che qualcuno usi un user_name diverso dal nome file,
			ma mi ha anche causato un mucchio di confusione nel software (in particolare la confusione si accentra intorno alla series_names_list di FormCompare). 

		4) Per quanto abbia separato decentemente calcoli da visualizzazioni ci sono ancora dei casini.
				
				In FormCompare, contrariamente a tutte le altre functions di questo genere, display_report e display_additional_report non hanno una variante calculate_ e una display_
				avevo impostato male la cosa fin dall'inizio e la gestione del calcolo è troppo inframmischiata a quella della visualizzazione perchè valga la pena di sistemarla adesso

				In FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
				assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpagliate quà e là. Non l'ho sistemato perchè c'è il rischio di
				infilarsi in qualche problema di Invoke, MA ME NE DEVO RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
				sono scritti meglio, USARE QUELLI PER IL TEMPLATE!!

		5) Per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
			'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
			MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
			I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE. AHHHH E I NOMI GIUSTI SAREBBERO EV_float, EV_long, EV_float_2d etc.!!!
			Inoltre ho tante di quelle varianti delle EVconvert() che non è facile starci dietro......

				IN GENERALE, GLI EVALUE SERVONO +- A QUALSIASI SOFTWARE.. PENSARCI ANCHE PER IL TEMPLATE

		6) Ho quintali di codice duplicato un po' dappertutto (in particolare nelle visualizzazioni, ma non solo)

		7) Bugs:

			Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
			il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
			FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

			Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
			finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
			semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
			chiusa se si lancia una nuova analisi o un nuovo compare...  IN GENERALE, IL DISCORSO DI COSA DISABILITARE E QUANDO ANDREBBE RIVISTO COMPLETAMENTE

			Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.


	- NUOVE FUNZIONALITA'

		- Se si vuole trovare le parole inusuali in molti testi la cosa è molto scomoda, dato che ogni volta bisogna caricare il reference e poi scegliere un corpus con
			cui comparare. Sarebbe bello introdurre una funzione 'Compare with corpus...' che eviti di dover selezionare ogni volta lo stesso file. Serve una variabile per
			il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova nullo, che direi di salvare in config e di visualizzare in options.
			UTILE!

		- Funzione 'Aggregate' per riunire assieme più files .txtalysis in un'unica analisi. Richiede la sistemazione di grovigli software #1, poi dovrebbe essere relativamente
			semplice. ACCESSORIO

		- Funzioni 'Analyze and Check': come le Analyze, ma esegue anche una comparazione col corpus di riferimento e visualizza la finestra
			additional report, in modo che si possa verificare subito se si vedono parole troncate o strane che fanno supporre problemi di ortografia
			nel testo sorgente. Serve una variabile per il filename del corpus + una funzione di menu' per settarlo, lanciata in automatico se lo si trova
			nullo quando si inizia un' Analysis and Check, che direi di salvare in config e di visualizzare in options. ACCESSORIO

				Dopodichè potrei fare una "Mass analyze, check & save": similmente a Mass Analyze, per ogni file .txt lancia l'analisi, poi la comparazione
								col corpus di riferimento e visualizza la pagina parole inusuali. Poi servono due pulsanti: 'accept' salva il file .txanalysis con lo
								stesso nome del .txt, 'don't accept' modifica il nome su disco del .txt incriminato per revisione successiva. ACCESSORIO

		- Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
			SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
			FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
			nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
			SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

		- Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
			PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO



	- MIGLIORIE

		- Scarto delle abbreviazioni composte da un char + '.' Comando Regex direi... SAREBBE UTILE!!!!

		- Find best matches non genera dei 'bei' grafici 2d di riepilogo, ma questo perchè, trovando testi a distanza ravvicinata, la scala Z dei grafici è molto
			espansa ed evidenzia anche le minime differenze nei testi. Bisognerebbe poter aggiungere un'outlier... ma va visto bene come sceglierlo
			(p.es. che abbia una distanza del doppio? del testo più distante fra i 24 selezionati? (ci si può anche annotare la lista dei testi
			e poi fare un Compare manuale aggiungiendo anche un outlier... ma selezionare i 24+1 testi per il Compare è una menata più unica che rara).			

		- Nel report del Compare un testo codificato viene correttamente identificato come non appartenente ad alcun cluster unblinded, 
				viene messo nel cluster _blind giusto (e anche _blind_dubious è ok), ma quando viene scritto il cluster di cui il testo NON fa parte vengono
				scritti i dati _unblinded, mentre sarebbe molto meglio scrivere quelli _blind 

		- Find best matches usa sempre il default di rare_characters_cutoff. Renderlo programmabile anche per find best matches non è semplicissimo perchè
				non esiste una textBox che lo può fare (nè ha senso aggiungerne una globale in Analisi per solo questa funzione). Chiedere il valore quando
				si lancia find best matches sarebbe una scocciatura quando lo si usa. Forse un'opzione sarebbe meglio. Anche max_output_results è fisso
				(viene dichiarato nella getsione di find best matches, evento Click su findBestMatchesInToolStripMenuItem). Anche qua un'opzione sarebbe credo l'unica
				soluzione sensata.

		- Migliorare il sort delle tabelle dati 2d di riepilogo in FormCompare (i grafici colorati rosso-giallo-verde) in modo da far vedere non solo quali testi sono nello
					stesso cluster del testo di riferimento base, ma anche se esistono eventuali clusters di altri testi. P.es.: adesso la tabella visualizza un quadrato
					verde in alto a sinistra dove ci sono le lingue che clusterano col testo base. Nel resto della tabella si nota che alcune della altre lingue sarebbero
					in-cluster fra di loro (perchè ci sono dei quadratini verdi), ma dato che non sono ordinati si fa fatica a vederli. Bisognerebbe ordinare la tabella
					secondo il cluster base solo fino ad un certo punto, poi ordinare il resto secondo i clusters 'locali'. FORSE LO SI PUO' FARE CERCANDO
					DI METTERE I QUADRETTI VERDI VICINO ALLA DIAGONALE DELLA TABELLA... E FORSE NELLO STESSO MODO SI POTREBBE SORTARE ANCHE IL CLUSTER BASE,
					INVECE DI COME FACCIO ADESSO. ALGORITMO INTERESSANTE.....

		- hmmm... se nessun cluster unblinded ma ce n'e' uno dubious sarebbe bello fare l'analisi unusual words su quello... MA OCIO PERCHE' HO RICHIAMATO
			   PER NOME I DATI DI BASE (_unblinded etc.) IN TANTI PUNTI MI SA....... E CI SARANNO PROBLEMI ANCHE CON L'IDENTIFICAZIONE DEL FILE
			   DA COMPARARE PROBABILMENTE... STARE ATTENTI...

		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica BELL'AGGEGGIO
				-Come accessorio alla decrittazione  vedere se può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto...). Magari nel report?

		- Visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica!! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tuti i casi visualizzare olo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE
			

		- In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). Tabellina da alcune prove fatte:

								Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

					BESTIA DI MODIFICA CHE COMPLETEREBBE IL SOFTWARE, MA CHE HA COME PREREQUISITO LA SISTEMAZIONE DEI GROVIGLI, SENNO' E' UN SUICIDIO

	- In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina
					
	- Avviso nell'additional reporty vocabulary su cosa fare se si vedono parole strane (c'è il file di pre-processing per eliminare i caratteri fallati? se c'è ma non
		funziona è un problema di codifica: aprire il file e sostituire manualmente i caratteri fallati. se sono proprio fallati in origine nel file buttarlo via..) 
		IL MAX SAREBBE UN PULSANTE DI HELP LOCALE...

	- Analisi/vocabolario: inserire un numero progressivo quando visualizzo le parole del vocabolario per le parole. NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA
			text_analyzer.analysis_results.vocabulary_words_distribution in un EValue_extended per aggiungere nell'element_additional il numero progressivo. la lista
			è lunga ci si può impiegare del tempo. BOH, FORSE E' UNA COSA CHE NON SERVE NEANCHE POI TANTO, E COMUNQUE ANDREBBE DOPO AVER SISTEMATO UN PO' DI GROVIGLI
			
	- Il vowel_set è una menata... e si allunga sempre più... non ci sarà una function 'di sistema' per capire se un carattere è una
			vocale o una consonante? mi pare di aver cercato e non era facile.... e poi è tanto lavoro per le pseudosillabe... anche se sono carine hehe	     

	- Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH...
	
	- Dato che le statistiche dipendono dai graphs-limits, si potrebbe provare a ridurre i limiti per vedere se si ottengono matches migliori? Mah, può essere
	  un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. OSSERVAZIONE: USANDO TUTTI I CARATTERI INVECE CHE LIMITANDOLI
	  LE STATISTICHE UNBLINDED IN-CLUSTER DEI BIGRAMMI MIGLIORANO LOL (QUELLE OUT-CLUSTER NON CAMBIANO SIGNIFICATIAMENTE) ANCHE ALTRE STATISTIHC EI COMPORTANO ALLO STESSO MODO
	  MA NON HO FATTO UN'ANALISI SISTEMATICA
	

	- Grafici compare 2d: sarebbe bello scrivere le labels di colonna A DESTRA usando i nomi delle lingue e non i numeri. Serve di abilitare la scrittura delle labels dei
		grafici 2d sia a sinistra che a destra, e usare due parametri diversi per limitare la lunghezza che viene scritta [così a sinistra si scrive solo il #, a
		destra numero + nome]. E va verificato che funzioni la separazione fra labels orizzontali e verticali data dagli EValue_2d (speriamo...) NON SEMPLICISSIMA
		MA MIGLIORA DI MOLTO LA PRESENTAZIONE DEI GRAFICI 2D DEL COMPARE

	- Grafici1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA DI MOLTO LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE


	- Grafici 1d: il congiungimento dei punti nel caso sia attivo l'istogramma ma il grafico venga visualizzato senza le labels non è un gran che perchè
		genera dei tratti inclinati, quando dovrebbero essere verticali SI NOTA MOLTO COMPARANDO I GRAFICI DEI COMPARE (ES. LE WORDSLENGTH DISTRIBUTIONS)
		CON QUELLI DELL'ANALISI, E' PROPRIO BRUTTA.. DEL RESTO PERO' EVITA CHE LE LINEE DI CONNESSIONE SI SOVRAPPONGANO TROPPO NEI GRAFICI A SERIE MULTIPLE..
		PROVARE A VEDERE COME STA VERTICALE? SE NON VA BENE PER LO MENO AUMENTARE LA PENDENZA??

	- Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	- Grafici1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	- Grafici1d: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	- Grafici2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	- Grafici: eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...





	- mancano un mucchio di pulsanti 'Select all', e inoltre c'è il famoso bacherozzolo del .Select: se si è già sulla textBox col cursore non funziona!


	   
	Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del tipo di grafico
	visualizzato (1d o 2d),	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMKENTE IMPORTANTE
	NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS' Già che siamo in argomento:
	sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
	E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
	  cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
	  NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
	  LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)	

	Sempre in EvoTemplate c'erano le pictureBox scrollabili: non mi ricordo proprio come le usassi ma direi che possano tornare utili, vedere! male che vada si potrebbe
		abche infilarle nel prossimo Template SFIZIO


	Inserire trackbars + caselline numeriche per settatura limiti min e max delel scala Z dei grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	Nella pagina Analisi/Chars Distances potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter se voglio anche far vedere i grafici lineari,
	  ma chissenefrega, potrei continuare a visualizzare i garfici 2d ma esportare nella textBox i dati 1d per poterli poi copiare in Excel ). Per uniformità andrebbe fatto anche nella
	  previous/following.... E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUZIIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	  DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA... en passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	  sia il caso di aggiungerli causando infiniti problemi di compatibilità coi files .txalysis) . BOH... NON SO QUANTO SIA UTILE, VEDERE

	Pulsante e casella di testo per settare il limite dei grafici 2d anche nella pagina Analisi usando il rare_characters_cutoff come si fa col Compare
		(e salvataggio del file .cfg del rare_character_cutoff). OVVIAMENTE MANTENERE ANCHE LA TRACKBAR E LA CASELLINA DI TESTO CHE CI SONO ADESSO, E' SOLO
		UNA FUNZIONE AGGIUNTIVA!!




SCARTATE:

	Sarebbe proprio bello avere anche il il Compare-Report 2d, oltre che l'1d. MA NO! SERVE A NADA, BASTA ANDARE A VEDERE LA PAGINA BIGRAMMI E LI' C'E' GIA' IL
				GRAFICO 2D DESIDERATO.. CON TUTTE LE LINGUE IN COMPARAZIONE RAFFRONTATE FRA LORO.

    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... EVITARE COME LA PESTE...
	 MODIFICARE LE OPZIONI FRA UN FILE E L'ALTRO PUO' CAUSARE CASINI CON LE STATISTICHE (C'E' UN AVVISO APPOSITO) Mancano anche le altre opzioni che avevo immaginato
	 (set author, set title...) ma non so se valga la pena di implementarle davvero. DIREI PROPRIO DI NO! EXTRAMENOSE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA ANCHE PERCHE' I CRITTOGRAMMI VENGONO GIA' RICONOSCIUTI BENE, DIREI PROPRIO DI LASCIAR PERDERE

-------------------------------------------------------------------------------

TEXT ANALYZER 010 012

OBIETTIVI/SVILUPPO

Iniziato il 04 Maggio 2024

Questa è la ToDo list da Text Analyzer 010 011, prioritizzata e as-built:

	1) Con poco lavoro inserisco tutte le altre pagine Compare basate su grafici 2d :) 04/05/2024

	2) Inserita gestione, checkbox e casella di testo per rimuovere i caratteri 'rari' dall'analisi nelle pagine Compare. E direi funzioni
		molto bene :) 04/05/2024

    3) Revisione completa di XGraphs1d! 05/05/2024 primo pomeriggio.
			- Gestione sgrovigliata direi abbastanza bene
			- Aggiunti i grafici a più di una serie (non ancora provati, e mi manca ancora la legenda delle serie)
			- Aggiunta un'opzione per congiungere i punti del grafico in una linea continua

	4) Prima di iniziare con le pagine Compare 1D inserisco la distribuzione della lunghezza parole nel vocabolario (Analisi/Vocabulary, con una checkBox)

	5) Inserisco il primo grafico 1D multiplo (pagina Compare-Vocabulary), debuggo tutto e inserisco la legenda nei grafici 1D. 05/05/2024 sera, mitico!

	6) Sistemo alcuni bugs dovuti a casi limite, il più importante e rognoso in XGraphs2d con grafici dove il range dei valori è quasi zero (con range
		 dei valori zero era già a posto, ma anche 'quasi' zero dava rogne. vedere commenti in XGraphs2d a min_allowed_absolute_range) 06/05/2024 mattina

	7) Inserire calcolo distanze anche con le variazioni vs. theoric dei bigrammi. 06/05/2024 mattina

	8) Inserita nella pagina Compare la gestione dei limiti dei grafici lineari con trackbar + casella di testo . Ricordare che si applica solo a vocabolario,
		pseudosillabe, words lengths. Inoltre adesso scrivo nel grafico globale se i dati sono stati limitati da graphs_limits_1d. 06/05/2024 pomeriggio. Poi inserito il
		calcolo delle distanze 1D unblinded (bene, era rognoso) e l'avviso che i calcoli sui dati 1D sono limitati da graphs_limits_1d (sia nella textbox
		che nella picturebox di riepilogo). E già che ci sono inserisco subito anche l'avviso sui 'rare characters' per i calcoli 2D in textboxes e
		pictureboxes di riepilogo. 06/05/2024, quasi ora di cena.

	9) Inserisco anche le pagine Compare words lengths e pseudo-sillabe. 06/05/2024 sera. Poi il 07/05/2024 mattina inserisco anche la pagina dei monogrammi (con
			la gestione specializzata dei limiti sui rare characters e non sui limiti dei grafici 1d). Ottimo. Poi inserisco anche la gestione degli splittersMoved.
			07/05/2024 sera: sistemati alcuni aspetti software e fatte varie prove: mi sono chiarito abbastanza le idee sul report finale del Compare, direi

	10) Inserito il report finale del Compare (un report testuale + grafico unblinded e grafico blinded). Al momento usa solo la statistica dei bigrammi,
			ma è concepito per poter usare una statistica qualsiasi (quando aggiungierò una comboBox per poterle scegliere...). I files vengono separati in clusters basandosi 
			semplicemente su delle soglie per le distanze: una soglia 'ACCETTATO' sotto alla quale si è nel cluster, e una soglia RIFIUTATO sotto 
			alla quale si è in un 'cluster dubbio'. I valori da uasare li ho stabiliti con delle pre-prove e sono nella tabella qua sotto (notare che
			i valori dipendono dall'opzione rare_characters e non ricordo come fosse settata, ma penso fosse OFF: en passant i valori delle distanze
			in-cluster migliorano mettendola OFF)

							Caso unblinded:
																ACCETTATO	RIFIUTATO
								Bigrams distances unblinded:	< 0.034		> 0.065		Ratio 1.9:1		BEST [in effetti ho poi usato 0.042 - 0.070]
								 Bigrams vs. theoric unblinded:	< 53.74		> 61.69		Ratio 1.15:1	molto scarso/scarsino e, PEGGIO, INSTABILE perchè dipende fortemente dai caratteri rari
								 Following char unblinded		< 1.94		> 2.04		Ratio 1.05:1    molto scarso, non è indipendente dai bigrams
								 Previous char unblinded:		< 0.95		> 1.39      Ratio 1.46:1    buono, ma non è indipendente dai bigrams
								 Distances following unblinded:     --------
								 Distances previous unblinded:	< 1.66		> 1.97      Ratio 1.18:1	scarso
								Single chars unblinded:			< 0.042		> 0.064		Ratio 1.52:1	BUONO

									+ conoscenza di quale carattere è 'space' [che dovrebbe essere il carattere più frequente!]

								Single chars nospace unblinded:	< 0.045		> 0.075		Ratio 1.67:1	BUONO
								Vocabulary unblinded:			< 0.065		> 0.081		Ratio 1.24:1	scarsino
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded però!
								Words length in vocabulary:			--------

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel unblinded:	< 0.045		> 0.054		Ratio 1.2:1		scarsino
								Syllables single-vow unblinded:	< 0.016		> 0.017		Ratio 1.06:1	molto scarso

							Caso blinded: 

								Bigrams distances blinded:		< 0.055		> 0.075		Ratio 1.36:1	BEST [in effetti ho poi usato 0.059 - 0.078]
								 Following char blinded:		< 2.32		> 2.45		Ratio 1.05:1	molto scarso
								Single chars blinded:			< 0.031		> 0.035		Ratio 1.13:1	molto scarso/scarsino							
								Tutti gli altri:					-----------
								
									+ conoscenza di quale carattere è 'space'  [che dovrebbe essere il carattere più frequente!]

								Single chars nospace blinded:	< 0.031		> 0.033		Ratio 1.06:1	molto scarso
								Words length in text:			< 0.075		> 0.084		Ratio 1.25:1    scarsino   blinded = unblinded	

									+ conoscenza di quali simboli sono le vocali [in teoria possibile automaticamente]

								Syllables multivowel blinded:	< 0.014		> 0.014		Ratio 1:1		al limite
								Syllables single-vow blinded:	< 0.045		> 0.057		Ratio 1.27:1	scarsino

				La gestione determina automaticamente se un file può essere una crittografia a sostituzione semplice (se il file non è nel cluster unblinded,
					ma è nel cluster blinded) e direi che funzioni bene :). Ottimo! 08/05/2024 primo pomeriggio.

	11) Introdotta la pagina 'Additional report': se è stato trovato un cluster unblinded cerco le parole frequenti nel vocabolario che non sono frequenti nei testi di riferimento
			08/05/2024 quasi ora di cena. Funziona benino ma ho ancora degli aspetti da vedere.

	12) Cercando di creare un corpus Italiano coi files che mi ha dato Paola su un CD-ROM sono saltate fuori delle rogne spiacevoli (e completamente indipendenti dal
			mio software) che mi hanno fatto dannare:
			
			- Ho scoperto che i file .txt possono essere o in formato UTF8 (quelli creati da pèrogrammi recenti) o UTF7 (legacy). L'unico modo per distinguerli
				è aprirli con un editor esadecimale: gli UTF8 hanno all'inizio tre bytes $EF $BB $BF, cho scoperto sono il https://it.wikipedia.org/wiki/Byte_Order_Mark (o BOM),
				gli UTF7 non hanno niente
			- Per creare i files .txt dal CD di Paola, dove ci sono vari .doc e .rtf, ho aperto i .doc/.rtf con Word e poi li ho salvati in formato .txt Word li salva
				in formato UTF7 però.... e aprendo gli UTF7 come UTF8 i caratteri 'strani', p.es. quelli accentati, non vengono riconosciuti e defaultano al carattere
				$00 '�'.
			- Ho sistemato questo problema controllando subito se un file, aperto con UTF8, contiene i '�', nel qual caso lo riapro come UTF7, col che i caratteri
				accentati tornano giusti (avevo anche trovato un workaround: aprire il file UTF7, copiare tutto e incollare in un nuovo .txt, oppure darlo direttamente
				da Word).
			- Ciò nonostante dei files danno ancora problemi, p.es. Baricco - Senza Sangue.doc contiene un apostrofo '’' che 'scompare' quando si aprono i files. Ma
				in effetti non è che scompaia, è che viene intrpretato come un carattere di controllo (p.es. U+0091) che non viene visualizzato (questo l'ho scoperto
				copiando il file dalla finestra Raw Source Text a https://regex101.com/ , che si è rivelato utilisssimo per vedeere i caratteri 'invisibili').  Windows
				Notepad i files li aper giusti, quindi il metodo per risolvere anche questo problema c'è, si tratta solo di scoprire che metodo è... CHE NON E' DETTO SIA UNA
				COSA FACILE. Però sono riuscito a trappare il problema (questo il 09/05/2024) inserendo la ricerca dei caratteri di controllo con un Regex.Match nel momento
				in cui carico ogni file, e avvisando con un mdError.

				Per sistemare i files che danno errore il metodo più semplice è aprirli con Notepad e poi slavarli con nome scegliendo come codifica UFT8 o ancora meglio
				UFT8 con BOM (c'è una combobox appena a sinistra del pulsante 'salva')!
				
			- NOTARE inoltre che il metodo più semplice per verificare se un file ha problemi di questo genere è confrontarlo col corpus e controllare l'Additional Report Vocabulary
				per vedere se ci sono parole troncate o lunghe una sola lettera o simili! Con questo metodo si trappano anche problemi dovuti al testo in sè stesso, per esempio ho
				trovato un.pdf che conteneva cose come 'Voi soli vo[i]rreste morire' o 'l<i> hanno tagliato', che non è ovviamente possibile trovare o correggere automaticamente,
				ma che possono essere identificati con l'Additional report (se p.s. compaiono parole come 'vo' o 'rreste')

				CONCLUSIONI:

					- Se possibile partire da files nativi UTF8, e curati come uso della punteggiatura (ad averli...)
					- Il Notepad di Windos è utilissimo perchè consente di aprire un .text con una codifica qualsiasi e poi di salvarlo con nome scegliendo il tipo di codifica:
						scegliere UFT8 o, ancora meglio, UFT8 con BOM
					- Word invece è una scocciatura perchè non permette di scegliere la codifica con cui salvare i files .txt. [Non so se salva sempre in UTF7 o se
						la codifica dio salvataggio dipende dalla codifica del file .doc/.rtf originario]. Temo che la cosa migliore sia aprire il file .doc/.rtf, selezionare
						tutto e poi copiarlo in Notepad. Oppure salvare il .txt e poi, se causa problemi, aprirlo con NotePad e risalvarlo con la codifica giusta come detto sopra.
					- Files .pdf: Acrobat è inutile perchè non solo non salva i .txt, ma non ha neanche la funzione 'seleziona tutto'. Ergo la cosa migliore è usare 
						Word anche per i .pdf

						09/05/2024 sera (ma oggi sono stato a Nave, ho fatto poco, mò sistemo il readme per domani e poi vedo di convertire qualche altro file in .txt valà).


CHIUSURA

	10/05/2024 Molto bene!

		- Ho la gestione completa del Compare su tutte le statistiche
		- Ho il riconoscimento automatico della lingua, INCLUSO il caso in cui il testo sia stato criptato con un algoritmo di sostituzione semplice (e funziona pure se 
			la sostituzione non è perfetta, entro certi limiti, p.es. se due caratteri vengono riuniti assieme (cosa che mi è capitata causa problema coi comandi Regex
			di criptazione). Funziona anche se viene sostituito anche il carattere spazio (p.es. tutti gli spazi diventano 'z' e tutte le 'a' diventano spazi). Funziona
			anche se il testo finale non contiene nessuno spazio e si riduce ad una parola unica (p.es. spazio diventa 'Ø' e nessun altro carattere diventa spazio). Ps.: 
			l'intera parola che forma il testo viene visualizzata in Analisi-Vocabolario, quindi se il testo è lungo ci vuole molto tempo di esecuzione, occhio
		- Ho una bozza dell'estrazione automatica delle parole frequenti in un testo ma rare nel corpus dei testi. Però va ritoccata!


		Ho una quintalata di cose da fare ancora... ma decido di chiudere qua la versione per non correre rischi



BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO

		-analisi/vocabolario: inserire un numero progressivo per le parole NON E' BANALE PERCHE' DOVREI COPIARE TUTTA LA LISTA EVconvert(text_analyzer.analysis_results.words_distribution)
			E AGGIUNGERE IL NUMERO PROGRESSIVO ALL'ELEMENT PRIMA DI PASSARLA A display_1d_data (POTREBBE DIVENTARE DI EVvalue_2d, pero' e' un brutto nome in questo caso), IL
			CHE PUO' RICHIEDERE DLE TEMPO CON VOCABLOARI LUNGHI. OPPURE MI SERVE UN FLAG PER LA display_1d_data che aggiunga il numero in automatico (COL CHE SI POTREBBE
			FARLO NON SULL'INTERO VOCABOLARIO MA SOLO SUI RECPORDS EFFETTIVAMENTE VISUALIZZATI)
		
		con le words-length: inserire una parola di esempio per ogni lunghezza di parola ANCHE QUESTO E' UTILE PER IL DEBUG	DEI FILES DI TESTO, E PER SCOPRIRE CHE CAZZO E'
			LA PAROLA DI 54 LETTERE CHE HO NEL CORPUS ITALIANO LOOOOLLLL, FARLA SUBITO CHE SONO CURIOSO HAHAHAHAH. PERO' ANCHE QUESTO NON E' BANALE PERCHE' LA PAROLA DI
			ESEMPIO DEVO ESTRARLA MENTRE CREO LA DISTRUIBUZIONE WORDS_LENGTH E QUINDI DEVO MODIFICARE LA STRUTTURA DI words_length_distribution FACENDOLA DIVENTARE
			DI EValue_2d (CHE PERO' SAREBBE UN BRUTTO NOME IN QUESTO CASO), IL CHE IMPLICA ANCHE DI MODIFICARE I FILES .TXA. INOLTRE DEVO MODIFICARE LA DISPLAY_1D_DATA ANCHE QUA TEMO

						Ergo... già che ci siamo:

				La words_length_distribution-vocabulary è calcolata on-the-fly, ma rischia di diventare un'operazione lunga con vocabolari molto lunghi... andrebbe
				calcolata di base e salvata nel.txa (ma la menata è che bisogna riscrivere tutti i .txa già creati... due coglioni coi dati titolo-autore etc... e se me
				ne sbattessi di questi dati???). Cercare get_words_length_distribution_in_vocabulary	(richiamata in Form1 e FormCompare) EHHHH... MA SAREBBE UTILE...
				E PRIMA la faccio meglio sto..



		- uhhh cristo... loaded_text viene salvato nei files .txa.... (l'ho dimenticato perchè non viene visualizzato) routine check_save_options in Form1, però bisogna
			anche aggiungere un'opzione... SCOCCIATURA MA PRIMA LO FACCIO MEGLIO STO!!! ANCHE PER MOTIVI DI COPYRIGHT....

		- e sempre con modifica dei .txa: migliorare le pseudo-sillabe spezzando le consonanti intervocali in due clusters, metà attaccati al nucleo precedente 
		    e metà al successivo GROSSA ROTTURA DI CAZZO PERCHE' LA MODIFICA ALL'ALGORITMO NON E' BANALE, MA MEGLIO FARLA SUBTIO PER AVERE I FILES .TXA GIUSTI...





		- SCEMENZA: NON SCRIVO 'NESSUN'ALTRA INFO' nell'Additional report se trovo solo un cluster 'blind' (caso del file da decrittare), come mai? ahhh... caso non ancora gestito :)

		- visualizzare anche il loaded_text in una pagina di Analisi per verificare più facilmente cosa si è caricato effettivamente in caso di problemi di codifica!! Inoltre
			modificare la gestione delle stringhe che non vengano scritte se superano i 300K: in tuti i casi visualizzare olo qualche migliaio di chars (poi c'è l'opzione
			se li si vuole tutti) FACILE


		- Additional report Vocabolario: ci sono due parametri nel software: words_to_examine e max_ratio (max_ratio è anche 'invisibile' dall'esterno, oltre che a essere
			diverso da testo a testo). Bisognerebbe che	l'utente possa modificare words_to_examine, max_ratio è più un casino perchè è un parametro balordo.

				HMMM HMMM, E SE CERCASSI LA PRIME K PAROLE NELL'INTERO VOCABOLARIO, E POI CONFRONTASSI LE FREQUENZE??? SAREBBE SICURAMENTE UN ALGORITMO PIU' 'LINEARE' E A QUESTO PUNTO
					MAX_RATIO NON SERVE PIU', SOSTITUITO DA UNA SOGLIA (x10, x20) CHE PERO' E' FACILE DA INTERPRETARE, SCRIVERE E MODIFICARE (DATO CHE E' UN PARAMETRO UNICO). IL PROBLEMA E' CHE MI
					TOCCA TRASFERIRE TUTTO IL VOCABOLARIO TARGET IN UN DICTIONARY INVECE CHE SOLO QUALCHE MIGLIAIO DI RECORDS... E TEMO CHE SIA UN'OPERAZIONE LUNGA CON VOCABOLARIO CON
					QUALCHE CENTINAIA DI MIGLIAIA DI PAROLE... PERO' LA MODIFICA DOVREBBE ESSERE ABBASTANZA SEMPLICE... (MI TOCCO)
						
						MA PRIMA: STAMPA 'VERBOSE' DELL'ADDITIONA REPORT VOCABULARY COSI' CAPISCO MEGLIO COSA SUCCEDE (CON AGGIUNTA DELE # PROGRESSIVO DELLA PAROLA NEI DUE
						VOCABOLARI E DELLE DUE FREQUENZE)
		
					MIGLIORARE IL WARNING SULL'ADDITIONAL REPORT VOCABOLARIO NEL CASO CI SIA PIU' DI UN TESTO DA COMPARARE NEL CLUSTER. ANZI, NON SO SE HA NEMMENO SENSO FARE
					L'ADDITIONAL REPORT IN QUESTO CASO..

		-analisi/vocabolario: inserire un numero progressivo per le parole, con le words-length: inserire una parola di esempio per ogni lunghezza di parola ANCHE QUESTO E' UTILE PER IL DEBUG
			DEI FILES DI TESTO, E PER SCOPRIRE CHE CAZZO E' LA PAROLE DI 54 LETTERE CHE HO NEL CORPUS ITALIANO LOOOOLLLL

		- ricordare che ho il dubbio di avere degli apostrofi non-standard al contrario oltre a quello che correggo col file di pre-processing... e se ci sono non me ne accorgo perchè
			spariscono dal testo finale quando viene ripulito (a meno che non generino parole 'strane' visibili nel Compare Additional Report). Fino ad'ora non ce ne sono
			(ho confrontato sostituendo/non sostituendo l'apostrofo 'al contrario' '‛' e non è cambiato neinte nell'analisi ), ma stare attenti, perchè qualcuno potrebbe
			averli usati come virgolette e soatituirli non andrebbe bene in quel caso. INSOMMA, URGE IL VERIFICATORE AUTOMATICO DEI .TXT VS. CORPUS, PER EVITARE DI FARE CASINI....


		- hmmm.. funzione automatica (un menù separato, direi), che dopo l'analisi fa un compare con il corpus e visualizza la pagina Compare/Additonal Report per
						il controllo sulle parole tronche o balorde (vedi sopra, problemi di codifica UFT8 UFT7 etc.). Dato che bisogna pre-analizzare e comparare una cifra di files .txt per validarli prima
						di usarli per creare il corpus aggiornato, serve una funzione che agisca su una directory, e direi sia meglio apra una Form separata da tutto il resto
						per i risultati.NON DOVREBBE ESSERE DIFFICILE MA DI SICURO E' LUNGHETTO

			Argomento correlato: l'additional report vocabolario è scomodo da usare, dato che ogni volta bisogna caricare il reference e poi comparare di nuovo col corpus. Sarebbe bello introdurre
			una nuova funzione in Analisi dove si carica il corpus e poi si comparano i vari files singoli, è che per come ho organizzato il software mi sa che è un casino... sigh
			però potrei: salvare il nome del file .txa col corpus, caricare nella classe di analisi il file .txa da comparare, poi lanciare il compare passandogli il nome file
			del corpus (che verrà ricaricato durante il compare) e alla fine ricaricare il corpus nell'analisi per mettere tutto a posto. Hmmm... neanche male....
		



		- Additional reports decrittazione: se è stato trovato un cluster blinded proporre una decodifica





	!! PROVE TESTI CODIFICATI SOSTITUENDO ANCHE LO 'SPACE': FATTA, OKAY!! vedere anche se l'identificazione automatica di 'space' col carattere più frequente può essere utile, o se 
		può essere utile avvertire che 'space' non è il carattere più frequente nel testo (cosa che mette comunque in sospetto). Magari nel report?

	Pagina di opzioni per poter modificare i valori delle soglie usati nel Compare (con restore defaults.. per carità). Ma dove la salvo?? BOH...

	In futuro: combobox nella pagina Compare-Report per scegliere quale statistica usare (adesso usa solo le Bigrams Distances). PERO' ATTENZIONE: VOCABULARY
		BLIND DISTINGUE VOYNICH BIOLOGICAL DAL COMPLETO (IN TUTTE E 3 LE TRASCRIZIONI), CHE E' UN'ECCEZIONE PERCHE' IN GENERALE VOCABULARY BLIND NON 
		DISTINGUE FRA LE LINGUE (NON COMPARE NEANCHE NELLA TABELLA QUA SOPRA). VEDERE IL GRAFICO 2D COMPARANDO I 6 VOYNICH BIO/COMPLETO NELLE TRE TRASCRIZIONI,
		NON E' FACILE DA SPIEGARE ED E' TARDI xD distanza max vocabulary blind (fra De bello gallico e Midway Revisited): 0.0468,
		minima (fra Dopo il divorzio e Regina dei caraibi): 0.00588. Distanza Voynich EVA Bio da Voynich EVA completo = 0.039 (simile a De Bello Gallico - Canterbury Tales 0.042
		o De Bello Gallico - Divina Commedia Inferno 0.037). Invece le altre statistiche raggruppano in tre clusters diversi le tre trascrizioni, anche se credo con distanze sempre
		sopra al limite ACCETTATO (ma è da verificare!!!) OK,NON E' UNA COSA CHE INFLUISCA SUL SOFTWARE, MA E' DA TENER PRESENTE PER IL VOYNICH!!!

	Dato che le statistiche dipendono dai graphs-limits, si potrebbe provare a ridurre i limiti per vedere se si ottengono matches migliori? Mah, può essere
	  un'idea, ma va pensata, anche perchè i tempi di elaborazione con tanti files da comparare diventano signficativi. OSSERVAZIONE: USANDO TUTTI I CARATTERI INVECE CHE LIMITANDOLI LE STATISTICHE UNBLINDED IN-CLUSTER DEI BIGRAMMI MIGLIORANO LOL
	  (QUELLE OUT-CLUSTER NON CAMBIANO SIGNIFICATIAMENTE)

	
	In Compare-WordsLength distribution la casellina 'blind' è ovviamente inutile.... POCO IMPORTANTE (CAUSA UN PO' DI CONFUSIONE PERO') facile da sistemare: basta disabilitarla
		nella pagina
	

	Grafici compare 2d: sarebbe bello scrivere le labels di colonna A DESTRA usando i nomi delle lingue e non i numeri. Serve di abilitare la scrittura delle labels dei
		grafici 2d sia a sinistra che a destra, e usare due parametri diversi per limitare la lunghezza che viene scritta [così a sinistra si scrive solo il #, a
		destra numero + nome]. E va verificato che funzioni la separazione fra labels orizzontali e verticali data dagli EValue_2d (speriamo...) NON SEMPLICISSIMA
		MA MIGLIORA DI MOLTO LA PRESENTAZIONE DEI GRAFICI 2D DEL COMPARE

	Grafici1d: sarebbe bella un'opzione per colorare quelli a singola serie con colori diversi in funzione del valore (sarebbero fighissimi nel report finale del
	   Compare: in-cluster = verdi, dubious-cluster = gialli, out-cluster= rossi NON SEMPLICISSIMA MA MIGLIORA DI MOLTO LA PRESENTAZIONE DEI GRAFICI 1D DEL
	   REPORT FINALE DEL COMPARE

	report finale del Compare in modalità 2D (tutte le distanze) MAH... TANTO LAVORO PER POCO RISULTATO MI SA 

	Possibilità di settare manualmente i limiti dei grafici 2d per ogni testo comparato: è che serve una pagina incasinata, e poi i limiti non li si può salvare
		  nel .cfg perchè variano in funzione dei testi caricati, ma si potrebbero salvare nella classe dei risultati del Compare. MAH, TROPPO LAVORO PER POCO FRUTTO

	Grafici1d: sarebbe bello aggiungere delle divisioni intermedie (se c'è spazio), sia sull'asse Y che su quello X ABBELLIMENTO
	Grafici1d: l'opzione oscilloscopio a volte fa sprecare troppo spazio, p.es. dati arrivano al max a 2.1 si sceglie 5.. sarebbe meglio 2.5. Vedere  ABBELLIMENTO
	Grafici2d: sarebbe bello aggiungiere delle divisioni intermedie sulla scala colori  ABBELLIMENTO
	Eh.. grafici interattivi.. almeno leggere il valore dal quadratino per i grafici 2D ad una sola serie, e poter clicckare sulla legenda dei grafici 1D a più
	   serie per disabilitare la visualizzazione serie per serie. GROSSO MIGLIORAMENTO, MA ANCHE GROSSO CASINO...


	In FormCompare ho gestito meglio i lanci di display_controls e update_graphs_limits_display rispetto a come è stato fatto in Form1. In FormCompare vengono lanciate
		assieme a enforce_control_coherency all'inizio della display_data, in Form1 sono sparpaglaite quà e là. Non l'ho sistemato perchè c'è il rischio di
		infilarsi in qualche problema di Invoke, MA ME NE DEVOI RICORDARE PER IL FUTURO TEMPLATE.... Inoltre: in FormCompare gli handlers degli eventi trackbar+textbox
		sono scritti meglio, USARE QUELLI PER IL TEMPLATE!!




Migliorie:
	   
	Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del grafico visualizzato...
	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE, MA SAREBBE PARTICOLARMKENTE IMPORTANTE
	NELLA FINESTRA COMPARE, DOVE IL LIMITE DEI GRAFI LINEARI NON AGISCE SUI MONOGRAMMI, DOVE INVECE E' ATTIVA L'OPZIONE 'RARE CHARACTERS' Già che siamo in argomento:
	sarebbe bello impedire di togliere il rare_characters_cutoff sulle pagine 2d che non siano i bigrammi (ha senso toglierlo solo sui bigrammi).
	E già che siamo in argomento Tabs: per semplicità sull'evento TabSelect lancio l'intera display_data (sia in Form1 che in FormCompare) e si nota che ci vuole più tempo. Sarebbe meglio vedere
	  cosa lanciare in funzione della pagina selezionata. MAH... DIREI CHE QUESTO UTLIMO PROBLEMA NON CI SIA PIU' DA QUANDO HO SEPARATO CALCOLI E VISUALIZZAZIONI
	  NEL COMPARE: VIENE RIVISUALIZZATO TUTTO, MA CI VUOLE POCO TEMPO. IDEM IN ANALISI. IN EFFETTI L'UNICO CALCOLO CHE E' RESTATO DOVREBBE ESSERE QUELLO DELLE DISTRIBUZIONI
	  LUNGHEZZA PAROLE NEL TESTO (CHE PREVEDO DI SPOSTARE E SALVARE NEI FILES .TXA)
	
    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... PUO' ASPETTARE,
	 ACCESSORIO Mancano anche le altre opzioni che avevo immaginato (set author, set title...) ma non so se valga la pena di implementarle davvero.

	Sempre in EvoTemplate c'erano le pictureBox scrollabili: non mi ricordo proprio come le usassi ma direi che possano tornare utili, vedere! male che vada si potrebbe
		abche infilarle nel prossimo Template SFIZIO

	Grovigli software: per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
		'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
		 MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
		 I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE

	Grovigli software: sono restati vari pezzi di codice duplicato nel calcolo delle statistiche. PUO' ASPETTARE E DIREI POSSA ANCHE NON SERVIRE DEL TUTTO

	Inserire trackbars + caselline numeriche per settatura limiti min e max grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	Nella pagina Analisi/Distanze potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter). Per uniformità andrebbe fatto anche nella
	previous/following... ma qua serve di meno. E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUIZIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA ANCHE PERCHE' DEVO SUDDIVIDERE LA FINESTRA DEI GRAFICI DELLE DISTANZE IN ORIZZONTALE O IN
	VERTICALE A SECONDA CHE STIA VISUALIZZANDO LE TABELLE O I DATI LINEARI. En passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	sia il caso di aggiungerli). BOH... NON SO QUANTO SIA UTILE, VEDERE

	Checkbox e casella di testo per rimuovere i caratteri 'rari' dall'analisi anche nelle pagine Analisi (e salvataggio del file .cfg). Boh, ma non farà confusione? VEDERE

	FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA.... DIREI DI LASCIAR PERDERE


Bugs:	

	Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
	il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
	FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

	Manca la gestione delle opzioni 'discard' su apostrofo e dash PUO' ASPETTARE, ACCESSORIO

	Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
	finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
	semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
	chiusa se si lancia una nuova analisi o un nuovo compare...

	Hmmmm... il save diretto del .txa (menù o Shift-F12) pare NON scriva il file nella directory giusta..., mentre save_as funziona. Controllare. HMMM HMMM CHE PALLE STE DIRECTORIES...

	Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.



Nuove funzionalità:


	Compare automatic: come quello manuale, ma legge tutti i files .txa di una directory selezionata e poi sceglie automaticamente i K files migliori per il display.

	Riconoscimento automatico delle vocali in Analisi&Compare  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE... IMPLICA UNA PAGINA IN PIU' DI PSEUDO-SILLABE
		SIA IN ANALISI CHE IN COMPARE (CON DUE TIPI DIVERSI DI NUCLEI, OVVIAMENTE) E LA MODIFICA DI ANALYSIS_RESULTS.. COL CHE BISOGNERA' AGGIORNARE I
		FILES .TXA.. a meno di calcolarla on-the-fly (dovrebbero bastare le frequenze dei bigrammi... ma meglio verificare!!!!!!! se serve il testo interno sono cazzi perchè
		nel .txa di solito non c'è). Se richiede troppo tempo di esecuzione sono cazzi però (a meno di lanciarla manualmente) PERO' E' ANCHE VERO CHE LE STATISTICHE FRA LE SILLABE
		SONO, BENE CHE VADA, SCARSINE, QUINDI SE CONVENGA TUTTO STO CASINO O NO, NON SAPREI PROPRIO DIRE....

	Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
	PUO' ASPETTARE, ACCESSORIO, INOLTRE MI SEMBRA CHE LE PSEUDO-SILLABE STIANO GIA' COSTANDO TROPPO LAVORO PER POCO FRUTTO DOPOTUTTO, SONO UNO SFIZIO

	Aggregate: aggrega più files .txtstats in un unico .txtstats. Occhio perchè qua entrano in gioco i problemi dei nomi e di come rioganizzare i vari files... INOLTRE per poter aggregare
				sarebbe meglio avere le OCCORRENZE, perchè con le sole FREQUENZE non lo si può fare. O modifico tutta la gestione inserendo nelle liste le frequenze invece delle
				occorrenze, e calcolo le frequenze on-the-fly nella display_data [che sarebbe l'opzione più radicale] o lascio tutto come sta ma mi assicuro di avere a disposizione
				il totale orioginale delle occorrenze per poi poter aggregare [più semplice come software, ma peggiore dal punto di vista della precisione numerica]. E parlando
				di precisione numerica... gli int sono una bella cosa ma arrivano solo fino a poco più di due miliardi... sarebbe una bella idea passare a long [non che sia immediato da fare]
				IN EFFETTI NON PARTICOLARMENTE UTILE DATO CHE IL PROCESSING DEI FILES DI TESTO E' DIVENTATO MOLTO VELOCE




-------------------------------------------------------------------------------

TEXT ANALYZER 010 011

OBIETTIVI/SVILUPPO

Iniziato il 20 Aprile 2024

Questa è la ToDo list da Text Analyzer 010 010, prioritizzata e as-built:

	1) Grovigli software: Form1 ripete una quantità di volte le stesse istruzioni nella display_data (sia textBoxes che plotters). Bisogna vedere se e quanto valga la pena di umanizzarla,
						questo prima di inserire tutti gli altri grafici che ancora mancano in varie pagine, ovviamente. LA SISTEMAZIONE DELLA DISPLAY_DATA E' DA FARE PER PRIMA PERCHE' BISOGNA
						POI INTERVENIRCI SOPRA PER POTER SISTEMARE IL PUNTO SEGUENTE. OK, sistemata direi decentemente, mi ci è voluta una mattinata più mezzo pomeriggio, 21/04/2024.

						Nel farlo trovo un bug rognoso: i calcoli della distribuzione bigrammi divisa per quella teorica erano sbagliati,
							causa problema di ordinamento delle due liste. OK SISTEMATO 21/04/2024.

	2) Problemi software. Problema dell'aggregazione delle frequenze e problema degli integer. Per poter aggregare le statistiche sarebbe meglio avere le OCCORRENZE,
				perchè con le sole FREQUENZE non lo si può fare. Soluzione radicale: modificare tutta la gestione inserendo nelle liste le occorrenze invece delle
				frequenze, che adranno poi calcolate on-the-fly nella display_data. Inoltre per tutti questi dati è meglio che passi al tipo long, perchè integer arriva
				solo fino a poco più di due miliardi, che in un programma come questo non è un gran che (:p). Contestualmente, devo anche rivedere parte del processo di
				analisi, sistemando Grovigli Software. OK, separate le occorrenze (che usano un long) dalle frequenze ed altri numeri, un'altra mezza giornata. Controllato
				che i dati siano identici fra versione 010 e versione 011 (lanciando contemporaneamente due versioni dle programma e confrontando direttamente i dati). 22/04/2024.

	3) Era sottinteso nella ToDo list: inseriti i grafici che mancavano. C'è voluto poco :) 22/04/2024

	4) Manca la gestione del refresh dei grafici quando si modificano le dimensioni della finestra (vedere EvoTemplate). Ok, c'è voluto un attimo (evento Form1.Refresh). 22/04/2024

	5)	Il grafico della distribuzione bigrammi effettiva divisa per quella teorica 'raw' è misleading, quindi al suo posto conviene scrivere un grafico nullo
		che spieghi nel titolo come mai non c'e' la	visualizzazione (lasciando comunque la possibilità di copiare i dati raw dalla textBox) AL MOMENTO HO SEMPLICEMENTE
		SETTATO L'OPZIONE 'SYMEMTRIC' A TRUE DI DEFAULT, CREDO CHE BASTI 22/04/2024 Ps: ho anche settato le opzioni 'Tabular' a tuyre di default, sono più belle a vedersi

	6) Sarebbe bello avere due opzioni (o due caselline da qualche parte) graphs_limits (che servirebbe anche per testare le routines dei grafici...). OK, fatta e funziona 
		bene devo dire, e c'è sia per i grafici 1d che per i 2d! Ho tribolato un paio d'ore prima che i controlli si comportassero come volevo ma il risultato è più che ok!
		En passant, avverto nel titolo dei grafici se vengono visualizzati meno dati di quelli disponibili. 22/04/2024

	7) 	Bug. Menata con la finestra di opzioni. Se si preme 'exit' o il quadratino 'X' di chiusura le opzioni vengono aggiornate ma il file di configurazione non viene salvato, causando
		un mucchio di confusione. L'idea originaria era che premendo 'exit' le opzioni originali non venissero modificate, ma per farlo bisogna prima salvarle da qualche parte
		e poi gestire l'evento Form.OnClose() o come si chiama. OK, SISTEMATO FACILMENTE RICARICANDO IL FILE DI CONFIGURAZIONE NELL'EVENTO FormClose (include il caso che
		si preme 'Discard and exit')
		
	8) Grovigli software: perchè ci vuole così tanto tempo per il 'formatting' finale dei dati in uscita con testi lunghi?? Investigare! Il problema è il vocabolario (22/04/2024),
	   bisogna limitare il numero di parole scritte nella textBox, con opzione per poterle scrivere tutte. A questo punto anche opzione per visualizzare i testi
	   anche se più lunghi di 300K caratteri (opzione di visualizzazione... il che vuol dire che serve una finestra in +). Mah... metterci anche l'opzione per numeri in
	   formato americano o europeo valà... OK, FATTO TUTTO E FUNZIONA ANCHE BENE 22/04/2024

	9) IDENTIFICATO E CORRETTO UN BUG DEL TEMPLATE 010 012: col load/save dei files .txa non viene aggiornata la directory nel file di configurazione. Mancava proprio
		l'aggiornamento e il salvataggio del file .cfg: li ho aggiunti nelle routines di gestione dei menù load, save e save as. Inoltre il salvataggio del file .cfg
		mancava anche nella parte custom, in load_and_analyze, dove viene aggiornata la user_text_files_directory. Bug identificato il 23/04/2024, sistemato il 30/04/2024
		dopo il ponte del 25 Aprile. Inoltre aggiunta la visualizzazione delle directories nella finestra Opzioni->Save files

   10) Di default non salvo source e cleaned text nel file .txa (sostituiti con una stringa di avviso), altrimenti i files potrebbero diventare enormi. Ci sono però due opzioni
		nella finestra Opzioni -> Save files per poterlo fare. La gestione c'era già al 23/07/2024, ho completato la finestra di opzioni il 30/04/2024.

   11) Nuovi XGraphics2d! Gestione rifatta completamente fra il 30/04 e 01/05/2024, e risultato direi niente male
			
			- Supportano un numero qualsiasi di serie di dati (non che abbia provato a visualizzarne più di una fin'ora, ma dovrebbe funzionare) PROVATO 02/05!! Mito!!!
			- Gestione completamente rifatta e, per quanto la routine DisplayGraph sia molto lunga, è organizzata direi bene
			- Sistemato sia un problema di scala colori (che era 'compressa' alla metà nelle versione precedente), che la visualizzazione della scala
				colori (aspetto rognoso dal punto di vista grafico). Inoltre la scala colori si autoadatta ai limiti effettivi dei dati
				presenti nel grafico (considerando eventualmente anche i valori di offscale_loiw e offscale_high), in modo tale da avere sempre la maggior risoluzione possibile.

	12) Introdotto il refresh dei grafici quando si muovono gli splitters. Con l'occasione ho anche rinominato tutti gli splitcontainers. Inoltre in inizializzazione
			divido gli splitcontainers dei grafici in due metà esatte 01/05/2024

	13) Aggiunto il calcolo delle entropie (in display_textual_tabs), che vengono visualizzate nella tab Report. Inserita entropia caratteri singoli (con e senza spazi),
		  entropia bigrammi e entropia parole. 01/05/2024

	14) Aggiunto il calcolo degli hapax legomena (è parte di analysis_results). Visualizzato nella pagina Report e nella textBox del vocabolario. Nella pagina Report
			scrivo anche qualche info aggiuntiva (#parole dizionario/#parole testo e #hapax legomena/#parole dizionario). 01/05/2024

		[02/05/2024 ho aggiunto un Compare minimale, che al momento visualizza solo i grafici delle distanze. Lol, mitico xD e i grafici 2d multipli funzionano bene!]

	15) Dato che mi sto portando avanti col compare, sistemo la gestione dei nomi etc., che era un punto spinoso! Alla fin fine decido per aprire una finestra ad ogni save 
		 (o save_as) di un file .txa, che visualizza e permette la modifica delle informazioni: nome dell'analisi (che verrà poi usato come nome durante il compare), titolo, autore,
		 anno di pubblicazione, lingua, note dell'utente. Inoltre aggiungo una voce di menù per visualizzarle (Show analysis info). Ottimo. 02/05/2024

	16) Megariorganizzazione delle routines display_data. Il problema era che avevo intrecciato la visualizzazione col processing dei dati (in particolare
		il passaggio da EValueOcc a EValue, nonche' la gestione della simmetrizzazione, e i calcoli delle distanze in FormCompare). Questo sarebbe stato
		molto spiacevole proseguendo col Compare e, soprattutto, col futuro Compare Automatico. Riorganizzo tutta la Form1, poi backuppo su chiavetta prima di continuare
		valà xD 03/05/2024 Ok, prima di mezzogiorno ho sistemato anche FormCompare.

	17) Inserimento della gestione 'unblinded' nel calcolo delle distanze 2d. Molto bene! fatta una verifica con Excel con due tabelle ortogonali (un testo in greco e
	      il Voynich Biologica EVA-ML) e i conti tornano :), poi ho anche provato con dei files minimali contenenti solo "a a a a..." e "b b b b..." e i conti tornano anche lì.
		  Le distanze calcolate sono sempre molto basse (anche coi due files più ortogonali possibile, "a a a a..." e "b b b b...", arrivo a 1.0017, lontano dal sqrt(2) massimo teorico.
		  Ma ripeto, i conti tornano sicuramente. Il comportamento delle distanze fra blinded e unblinded è quello che ci si aspetta: diminuiscono fra lingue uguali ma
		  restano +- uguali fra lingue diverse. 03/05/2024 sera, anzi quasi mezzanotte adesso dopo aver fatto tutte le varie verifiche sui calcoli (e sistemati due
		  buggettini incontrati per strada sui grafici 2d, che si vedevano solo coi files 'minimali'). Il 04/05/2024 mattina inserisco anche il calcolo delle distanze in
		  forma tabellare, nonchè l'ordinamento delle tabelle distanze (che non era semplice...), e nel primo pomeriggio inserisco anche i grafici complessivi. Mitico!!



CHIUSURA

	04 Maggio 2024

	Che posso dire... 'exceeds expectations!'.

	Direi di chiudere qua la versione:

		- Ho la gestione completa dell'analisi (a parte qualche accessorio, vedi più sotto)
		- Ho impostato alla grande tutta la gestione del Compare, anche se per adesso limitata al Compare sui bigrammi, ma facilmente estendibile!

	Chiudo e passo alla 010 012



BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO

Migliorie:

	Proseguire col resto delle pagine di Compare con dati 2d 

	XPlotGraphs1d va assolutamente portato ad una forma umana, e vanno gestite le serie multiple.  Bel lavoretto... ma serve per inserire i Compares sui dati 1d.

	Report finale del Compare (tutto da vedere) Il metodo più sicuro è ricalcolare tutte le distanze (anche se l'ho già fatto quando scrivo le 
		varie pagine), comunque il tempo necessario è limitato. E CHE ACCADE SE VIENE LIMITATA LA LUNGHEZZA DELLE TABELLE? VABBE' RICALCOLO TUTTO, REPORT INCLUSO, E CI SCRIVO P.ES. CHE I CARATTERI
		SONO STATI LIMITATI A QUELLI CON FREQUENZA MINORE DI 1:500 O QUELLO CHE SARA'

	Hmmm pulsante 'shrink table size to remove rare characters'... mi sembrerebbe un'ottima idea... ma cosa vuol dire 'raro' ? Meno di 1 ogni 500 andrebbe bene per eliminare i diacritici
		dall'Italiano e gxvz nel Voynich EVA completo, non sarebbe neanche male. Potrei anche aggiungiere una casellina lì attaccata per programmare 1 ogni XXX chars.
		Nella pagina di Analisi, ovviamente, MA IL POSTO MIGLIORE
		DOVE AVERLO SAREBBE NEL COMPARE, DOVE NON METTEREI LE TRACKBARS, LASCEREI FARE TUTTO IL LAVORO AL PULSANTE 'REMOVE RARE CHARACTERS' + CASELLINA COL NUMERO. NOTARE ANCHE CHE
		NEL COMPARE HA SENSO RIMUOVERE I CARATTERI RARI ANCHE IN _ALCUNI_ DEI GRAFICI LINEARI (ES. I SINGLE CHARACTERS, MA OVVIAMENTE NON IL VOCABOLARIO O LA LUNGHEZZA
		DELLE PAROLE...), IL CHE COMPLICA LA VITA. VABBE',VEDREMO Inoltre: nei grafici multipli 2d (Compare) manca la scritta che avverte che i dati sono stati troncati. Cercare
		ATTENZIONE! MANCA GESTIONE AVVISO DATI TRONCATI (FormCompare.display_2d_graphs)

	Manca la distribuzione della lunghezza parole NEL VOCABOLARIO (as opposed to NEL TESTO). Aggiungere una checkbox per scegliere se visualizzare nel testo/nel vocabolario.
		PUO' ASPETTARE, ACCESSORIO, PALLOSO CAUSA CHECKBOX, MA E' UN DATO DI COMPARAZIONE....


    FormCompare.calculate_distances_2d: nel calcolo delle distanze 'blind' sarebbe bello introdurre un algoritmo che reshuffla righe/colonne della tabella su cui
	   calcolare la distanza in modo da minimizzarla. SAREBBE OTTIMO, MA NON E' SEMPLICE, CMQ HO GIA' PREVISTO L'ENUM BlindFactor2d CHE INCLUDE GIA' QUESTO CASO,
	   SI TRATTA 'SOLO' DI SCRIVERE L'ALGORITMO..... RICORDARE CHE CE' UNA FUNCTION reshuffle_rowcolumns GIA' SCRITTA (mai provata... ma dovrebbe essere utile per questo scopo)
	   BOH... MI SEMBRA ABBIA UN'UTILITA' LIMITATISSIMA.... DIREI DI LASCIAR PERDERE
	   
	Sarebbe bello abilitare/disabilitare le trackbar etc. dei graphs limits (in Analysis, nei Compare bisogan vedere cosa implemento) in funzione del grafico visualizzato...
	il che vuol dire dover accedere all'indice delle Tabs. PUO'	ASPETTARE MA RENDE IL PROGRAMMA PIU' FACILE DA USARE
	
    Manca la gestione dell'override delle opzioni di preprocessing tramite comandi nei commenti nei files di testo. L'override è facile, il restore già di meno... PUO' ASPETTARE,
	 ACCESSORIO

	Sempre in EvoTemplate c'erano le pictureBox scrollabili: non mi ricordo proprio come le usassi ma direi che possano tornare utili, vedere! male che vada si potrebbe
		abche infilarle nel prossimo Template SFIZIO

	Grovigli software: per i dati 2d bisognerebbe usare EValue_2d e non EValue...  è che ci ho pensato tardi. Vedere commento a EValue_2d in TextAnalysis_result, e commenti
		'QUI LA GESTIONE E' CONFUSA:' nelle display_2d_graphs in Form1 e FormCompare UH UH, MA ATTENZIONE PERCHE' L'ELEMENT UNICO DI EVALUE E' FONDAMENTALE  IN UN
		 MUCCHIO DI GESTIONI (PARTENDO DALLE CREAZIONI DELLE TABELLE), ERGO SERVE MANTENERE element IDENTICO A QUELLO DI EValue anche in EValue_2d,  E AGGIUNGERE
		 I DUE ELEMENTS SEPARATI DA USARE PER LE VISUALIZZAZIONI DI TABELLE E GRAFICI. PUO' ASPETTARE

	Grovigli software: sono restati vari pezzi di codice duplicato nel calcolo delle statistiche. PUO' ASPETTARE E DIREI POSSA ANCHE NON SERVIRE DEL TUTTO

	Inserire trackbars + caselline numeriche per settatura limiti min e max grafici 2d. Permette di evidenziare nei grafici un mucchio di cose, p.es. potrei restringere la scala
		delle distanze verso il basso in modo che tutte quelle sopra ad un certo valore diventano rosse, mentre la scala colori di tutte quelle sotto al valore viene espansa!
		Heheh il top dei top sarebbe poter trascinare due cursori direttamente nel grafico... ma non pretendiamo troppo, e inoltre avere ANCHE una casellina numerica è certamente
		utile per motivi di	precisione e flessibilità. MAH, NON SO CHE DIRE, IN DEFINITIVA LA VEDO ROGNOSA, LUNGA E POCO UTILE

	Nella pagina Analisi/Distanze potrei aggiungere la modalità 'lineare' (il che implica di dover modificare lo splitter). Per uniformità andrebbe fatto anche nella
	previous/following... ma qua serve di meno. E' UNA BELLA MIGLIORIA PERCHE' LA DISTRIBUIZIONE DELLE DISTANZE POTREBBE ESSERE IMPORTANTE PER IL VOYNICH (DOVE CREDO CI SIANO SIA
	DISTANZE ANORMALMENTE BASSE CHE TROPPE DISTANZE ELEVATE), PERO' E' MENOSA ANCHE PERCHE' DEVO SUDDIVIDERE LA FINESTRA DEI GRAFICI DELLE DISTANZE IN ORIZZONTALE O IN
	VERTICALE A SECONDA CHE STIA VISUALIZZANDO LE TABELLE O I DATI LINEARI. En passant i dati lineari vanno ricavati on-the-fly, non sono previsti in analysis-results e non direi
	sia il caso di aggiungerli). BOH... NON SO QUANTO SIA UTILE, VEDERE


Bugs:	

	Scemenza rognosa: salvando il file di configurazione quando si esce dalle finestre delle opzioni il .save scrive 'done' nella finestra di status ogni volta. Hmmm o forse è
	il load in FormClose che scrive il 'done', sarebbe da controllare. PUO' ASPETTARE MA SAREBBE INTERESSANTE SAPERE ESATTAMENTE COME FUNZIONA DATO CHE IN VARI PUNTI SALVO IL
	FILE .CFG E NON VEDO ALTRI 'done', MI SEMBRA

	Manca la gestione delle opzioni 'discard' su apostrofo e dash PUO' ASPETTARE, ACCESSORIO

	Bisognerebbe impedire di aprire una finestra opzioni se ce n'è già una aperta, altrimenti si potrebbero far casini p.es. modificando le opzioni nelle due
	finestre, poi premendo 'Save' su una finestra (col che vengono salvate anche quella dell'altra) e poi 'Discard and exit' sull'altra finestra. Hmmm... il modo più
	semplice mi sembra sia quello di disabilitare il menù 'opzioni' quando se ne apre una. PUO' ASPETTARE MA E' IMPORTANTE Ps.: anche la finestra 'Compare' sarebbe meglio fosse
	chiusa se si lancia una nuova analisi o un nuovo compare...

	Hmmmm... il save diretto del .txa (menù o Shift-F12) pare NON scriva il file nella directory giusta..., mentre save_as funziona. Controllare. HMMM HMMM CHE PALLE STE DIRECTORIES...

	Difetto di VSaveAnalysisDialog: premendo 'exit' le eventuali modifiche vengono comunque confermate. BUG.



Nuove funzionalità:

	Compare (in fase avanzata!): permette di caricare più files .txa (magari chiamarli .txtstats sarebbe anche meglio) e di compararli fra di loro. L'elaborazione non dovrebbe essere troppo difficile,
				ma il casino è presentare i dati. Il Compare potrebbe essere fatto 'blind', cioè senza supporre niente su quello che i caratteri rappresentino (p.es. che
				la 'a' italiana sia la stessa cosa della 'a' tedesca) o 'unblinded'. Notare che il 'compare' potrebbe portare all'identificazione automatica della lingua, soprattutto
				nel caso 'unblinded' dove forse basta anche solo la frequenza dei caratteri (così si potrebbero anche risolvere cifrari a sostituzione semplice).
				Si potrebbe anche salvare il compare effettuato in un file .txtcomp. Per sicurezza sarebbe opportuno salvare anche la data di ultima modifica dei files processati.
				DA DEFINIRE. IN EFFETTI FAREI CARICARE UN .TXT E POI LO FAREI COMPARARE CON UNA LISTA DI FILES .TXA (SELEZIONATI CON UN ALTRO DIALOGO DI LOAD). BISOGNEREBBE
				APRIRE UN'ALTRA FINESTAR PER I RISULTATI, PER NON DIVENTARE SCEMI. E MI SA CHE MI SERVONO GRAFICI A PIU' DI UNA SERIE (INCLUSI QUELLI 2D: VISUALIZZARE DUE
				RIQUADRI DI VALORI) ANCHE PER MANTENERE LA SCALA UGUALE FRA UNA SERIE E L'ALTRA. E VEDERE SE NON CONVENGA ELIMINARE SOURCE TEXT E CLEANED TEXT DAL SALVATAGGIO
				NEL .TXA... ERGO DEVE ASPETTARE LA SISTEMAZIONE DEI GRAFICI, E SAREBBE MEGLIO ANCHE IL RICONOSCIMENTO VOCALI AUTOMATICO....

					100% Blind: nessuna ipotesi su cosa rappresentino i caratteri. Notare che questo esclude analisi basate sul vocabolario, dato che non si sa nemmeno se
								esista un carattere 'spazio'
							  : si suppone che entrambi i testi contengano un carattere che rappresenta 'spazio' (e che sarà probabilmente il più frequente). Abilita le analisi
									basate sulla lunghezza parole nel vocabolario
							  : si suppone di avere identificato le vocali. Abilita le analisi basate sulle sillabe, anche se non credo abbia senso andare oltre la distribuzione
									delle frequenze (non ha proprio senso: so quali simboli rappresentano le vocali nei due testi, ma non so se i simboli corrispondano!)
					0% Blind  : si suppone che i caratteri rappresentino la stessa cosa nei due testi. ERGO QUA USCIAMO DAI CIFRARI A SOSTITUZIONE SEMPLICE. Abilita le analisi
									basate sulle parole del vocabolario (ie.: identificazione argomenti con parole rare in genere ma usate spesso nel testo)

				Compare manuale: far selezionare fino a K files .txa, poi displayare i vari grafici per comparazione. E mi sa che servirà una qualche casellina per settare un valore
					sotto al quale eliminare i caratteri 'rari' dalla comparazione.

				Compare automaticO: far selezionare quanti files .txa si vuole, poi vengono scelti automaticamente i K migliori per il display.

	Riconoscimento automatico delle vocali  PUO' ASPETTARE, MA ACCESSORIO CARINO, E MI SA CHE TORNA UTILE COL COMPARE...

	Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')
	PUO' ASPETTARE, ACCESSORIO		

	Aggregate: aggrega più files .txtstats in un unico .txtstats. Occhio perchè qua entrano in gioco i problemi dei nomi e di come rioganizzare i vari files... INOLTRE per poter aggregare
				sarebbe meglio avere le OCCORRENZE, perchè con le sole FREQUENZE non lo si può fare. O modifico tutta la gestione inserendo nelle liste le frequenze invece delle
				occorrenze, e calcolo le frequenze on-the-fly nella display_data [che sarebbe l'opzione più radicale] o lascio tutto come sta ma mi assicuro di avere a disposizione
				il totale orioginale delle occorrenze per poi poter aggregare [più semplice come software, ma peggiore dal punto di vista della precisione numerica]. E parlando
				di precisione numerica... gli int sono una bella cosa ma arrivano solo fino a poco più di due miliardi... sarebbe una bella idea passare a long [non che sia immediato da fare]
				IN EFFETTI NON PARTICOLARMENTE UTILE DATO CHE IL PROCESSING DEI FILES DI TESTO E' DIVENTATO MOLTO VELOCE







	



-------------------------------------------------------------------------------------------------------------------------------------------------------------------

TEXT ANALYZER 010 010

OBIETTIVI

15 Aprile 2024



SVILUPPO

	Iniziato 15 Aprile 2024

	-- OBIETTIVI RAGGIUNTI --

	Wow, al 20 Aprile 2024 direi di avere tutta la base!

		- Pre-processing del testo sfruttando estesamente Regex, e inoltre posso anche definire dei comandi nei files di testo che lanciano
			dei search and replace di Regex su tutto il testo da esaminare (p.es. posso convertire il Voynich da EVA a EVA-Mauro lanzini, o il tedesco coi
			diacritici in tedesco senza diacritici etc. etc.). Grande.

		- Calcolo tutte le statistiche che avevo nel tool di analisi su Excel, e anche di più!
			- frequenza caratteri singoli (con e senza spazio)
			- distribuizione teorica dei bigrammi considerando solo la frequenza dei caratteri singoli (idea presa da https://www.voynich.nu/extra/sol_ent.html)
			- distribuzione effettiva dei bigrammi, sia in forma di lista ordinata che di tabella
			- distribuzione effettiva dei bigrammi divisa per quella teorica (ha il difetto che i bigrammi 'soppressi' rispetto alla distribuzione teorica hanno
				tutti valori compresi fra 0 e 1, quindi valori molto piccoli rispetto ai valori (maggiori di 1, e anche di tanto) dei bigrammi 'enhanched')
			- distribuzione effettiva dei bigrammi divisa per quella teorica coi dati modificati (viene preso il reciproco dei valori compresi fra 0 e 1, cambiato
				di segno), che consente visualizzazioni molto migliori dei dati
			- vocabolario, con distribuzione delle parole nel testo e distribuizione della lunghezza delle parole
			- estrazione di pseudo-sillabe usando meno informazioni possibili (solo quali simboli rappresentano le vocali, cosa che si potrebbe anche cercare di ricavare
				automaticamente dal testo), in due modi diversi: ogni vocale è un nucleo separato, oppure più vocali adiacenti formano un unico nucleo

		- Grafici: ho migliorato la classe GraphsPlotter di EvoTemplate 010 010, che è diventata XPlotGraphs1d. Adesso se c'è abbastanza spazio nel grafico può passare
						ad una modalità di visualizzazione tipo istogramma, con rettangolini invece che lineette per indicare i dati. Inoltre in questo caso le può essere
						passata una lista di stringhe che vengono usate come labels dell'asse X al posto dei valori numerici di default. Funziona piuttosto bene, anche
						se è aggrovigliata (come del resto era la GraphsPlotter originaria) :)
				   Inoltre ho introdotto XPlotGraphs2d ! Grafici che prendono una tabella 2d in ingresso (+ una lista di labels) e la convertono in una scala di colori
						(con tanto di offscale high e offscale low, cosa che migliora le visualizzazioni). Classe confusa quanto mai ma funzionante xD.


CHIUSURA

20 Aprile 2024

	C'è ancora un bel po' di lavoro da fare (vedi sotto), ma sono andato alla grande :)



BUGS E PROBLEMI CONOSCIUTI, E POSSIBILITA' DI MIGLIORAMENTO

Bugs:
	
	Menata con la finestra di opzioni. Se si preme 'exit' o il quadratino 'X' di chiusura le opzioni vengono aggiornate ma il file di configurazione non viene salvato, causando
		un mucchio di confusione. L'idea originaria era che premendo 'exit' le opzioni originali non venissero modificate, ma per farlo bisogna prima salvarle da qualche parte
		e poi gestire l'evento Form.OnClose() o come si chiama.

	Manca la gestione delle opzioni 'discard' su apostrofo e dash

Migliorie:

	Serve assolutamente un'opzione per poter esportare i dati in formato europeo o americano (col punto o con la virgola), e non la si può mettere fra quelle di 
		preprocessing perchè non c'entra niente. Vedere. Le istruzioni per settare la culture sono in Form1: NumberFormatInfo nfti = CultureInfo.CreateSpecificCulture("it-IT").NumberFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = nfti;
	
	Opzione per visualizzare i testi anche se più lunghi di 300K caratteri (opzione di visualizzazione... il che vuol dire che serve una finestra in +)

	Sarebbe bello avere due opzioni (o due caselline da qualche parte) per poter settare max_symmetric_value e min_symmetric_value

    Manca la gestione dell'override delle opzioni tramite comandi nei commenti nei files di testo

	Manca la gestione dell'autore, titolo e anno di pubblicazione dell'opera originale scritti come comandi nei files .txt [sono il minimo], poi anche 'lingua'? pensarci...

	Manca la gestione di user_assigned_name e user_notes (campi di textanalysis_results al momento), ma vedi anche Nuove Funzionalità / Aggregate

	Manca la gestione del refresh dei grafici quando si modificano le dimensioni della finestra (vedere EvoTemplate)

	1) Problemi software: vedi Aggregate più sotto. Problema dell'aggregazione delle frequenze e problema degli integer. Ma spostando il calcolo delle frequenze nella
			display_data bisogna prima affrontare 1a), sigh.

	Grovigli software: perchè ci vuole così tanto tempo per il 'formatting' finale dei dati in uscita con testi lunghi?? Investigare!

	Grovigli software: XPlotGraphs1d e XPlotGraphs2d vanno assolutamente portati ad una forma umana. E' un lavoro lunghetto (ci sono anche dei dettagli da correggere nei grafici,
						tipo barrette verticali nei grafici 1d o posizione delle labels della scala colori in quelli 2d, definire un mucchio di parametri aggiuntvi per font labels etc etc).

	1a)!! Grovigli software: Form1 ripete una quantità di volte le stesse istruzioni nella display_data (sia textBoxes che plotters). Bisogna vedere se e quanto valga la pena di umanizzarla,
						questo prima di inserire tutti gli altri grafici che ancora mancano in varie pagine, ovviamente. E inoltre il grafico della distribuzione bigrammi effettiva
						divisa per quella teorica 'raw' è misleading, quindi al suo posto conviene scrivere un grafico nullo che spieghi nel titolo come mai non c'e' la
						visualizzazione (lasciando comunque la possibilità di copiare i dati raw)

	Grovigli software: serve di dare una revisione a tutto il processo di analisi, e anche qua ci sono pezzi di codice duplicato. SALVARE IN EXCEL UNA COPIA COMPLETA DI UN PAIO DI
						ANALISI FATTE CON LA VERSIONE CORRENTE PER POTER POI VERIFICARE CHE SIA ANDATO TUTTO BENE!!!!!!!

	Manca il calcolo della/delle entropia/entropie.

	Manca la distribuzione della lunghezza parole NEL VOCABOLARIO (as opposed to NEL TESTO). Va modificata anche la tab del vocabolario temo.


Nuove funzionalità:

	Riconoscimento automatico delle vocali

	Separazione sillabe con ANCHE la scala della sonorità (ovviamente le informazioni richieste sono tante: bisogna sapere per ogni simbolo di consonante che consonante 'è')

	Compare: permette di caricare più files .txa (magari chiamarli .txtstats sarebbe anche meglio) e di compararli fra di loro. L'elaborazione non dovrebbe essere troppo difficile,
				ma il casino è presentare i dati. Il Compare potrebbe essere fatto 'blind', cioè senza supporre niente su quello che i caratteri rappresentino (p.es. che
				la 'a' italiana sia la stessa cosa della 'a' tedesca) o 'unblinded'. Notare che il 'compare' potrebbe portare all'identificazione automatica della lingua, soprattutto
				nel caso 'unblinded' dove forse basta anche solo la frequenza dei caratteri (così si potrebbero anche risolvere cifrari a sostituzione semplice).
				Si potrebbe anche salvare il compare effettuato in un file .txtcomp. Per sicurezza sarebbe opportuno salvare anche la data di ultima modifica dei files processati.

	Aggregate: aggrega più files .txtstats in un unico .txtstats. Occhio perchè qua entrano in gioco i problemi dei nomi e di come rioganizzare i vari files... INOLTRE per poter aggregare
				sarebbe meglio avere le OCCORRENZE, perchè con le sole FREQUENZE non lo si può fare. O modifico tutta la gestione inserendo nelle liste le frequenze invece delle
				occorrenze, e calcolo le frequenze on-the-fly nella display_data [che sarebbe l'opzione più radicale] o lascio tutto come sta ma mi assicuro di avere a disposizione
				il totale originale delle occorrenze per poi poter aggregare [più semplice come software, ma peggiore dal punto di vista della precisione numerica]. E parlando
				di precisione numerica... gli int sono una bella cosa ma arrivano solo fino a poco più di due miliardi... sarebbe una bella idea passare a long [non che sia immediato da fare]

	



-------------------------------------------------------------------------------


