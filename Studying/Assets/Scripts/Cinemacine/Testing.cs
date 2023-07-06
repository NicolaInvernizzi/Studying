using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

}

/*
 * Default camera di Unity <- collego Brain -> sovrascrive ciò che rendera la telecamera figlia (cinemacine, es. virtual) sulla default.
 * Il Brain può puntare al massimo ad una cinemacine camera.
 * Es. Virtual camera: oggetto vuoto con component virtual camera.
 * 
 * Posso inserire quante virtual camera voglio. 
 * Se attivo virtual2 mentre virtual1 attiva il brain esegue switch con blend verso la 
 * 2, solo in play mode.
 * 
 * Virtual Camera Component: 
 * 1. Lens: impostazioni della piramide della camera.
 * 2-3 vogliono un transform da seguire:
 * 2. Follow: segue la posizione del gameobject
 * 2.1. Body: algoritmi che determinano il movimento del Follow. Tracked Dolly = setto percorso specifico sul quale la camera
 *            si muoverà: Inserire Dolly Track con il suo Dolly Cart. In body -> AutoDolly enabled.
 * 3. LookAt: segue la rotazione del gameobject in modo da averlo sempre in vista.
 * 3.1. Aim: algoritmi che determinano la rotazione del LookAt.
 * 4. Status Live: Live = usata dal brain, Standby = non usata dal brain ma attiva,
 *    Disabled = The gameobject is disabled.
 *    Con Solo attivo la camera in modo da vedere rapidamente modifiche del transofrm, 
 *    utile per modificare al volo.
 * 4.1. Standby Update: quando camera in standby continua ad aggiornare e consumare
 *                      potenza di calcolo. 
 *                      Always = stesso peso di calcolo della camera attiva. Aggiornata al 
 *                      frame. Never = assenza di calcolo. Round Robin = più il numero di 
 *                      camere in standby aumenta e meno frequentemente verrà aggiornata.
 * 5. Game Window Guides: linee di riferimento in game view, visibili con body = Framing Transposer.
 * 5.1 Cambia Guides a seconda di Body e Aim.
 * 6. Saving during PlayMode salva sempre i cambi di settings del component.
 * 7. Se ho più camere in scena priority indica quale camera (priority maggiore) va considerata attiva tra quelle
 *    presenti. Abbassare la priority triggera un blend che esegue lo switch tra camere, solo in play mode.
 * 8. Blend Hint: tipi di switch tra camera. None = linear.
 * 9. OnCameraLive: evento che chiama metodo assegnabile come unity event. Verrà eseguito appena la 
 *    Camera sarà attiva. Ho sia eventi sul Brain sia sulla Virtual Camera.
 * 10. Noise: vibrazioni con presets e custom. 
 *    
 */
