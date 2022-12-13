import { useEffect } from 'react';

import { Helmet } from 'react-helmet';
import { useNavigate } from 'react-router-dom';
import { isMobile } from 'util/screen';

import { Unity, useUnityContext } from 'react-unity-webgl';

import back from 'assets/icons/back.png';

import classes from './Game.module.scss';

const Game: React.FC = () => {
	const { unityProvider, loadingProgression, isLoaded, UNSAFE__detachAndUnloadImmediate } = useUnityContext({
		loaderUrl: 'project/Web.loader.js',
		dataUrl: 'project/Web.data',
		frameworkUrl: 'project/Web.framework.js',
		codeUrl: 'project/Web.wasm'
	});

	const navigate = useNavigate();

	useEffect(() => {
		if (isMobile()) {
			navigate('/');
		}

		return () => {
			UNSAFE__detachAndUnloadImmediate();
		};
	}, [UNSAFE__detachAndUnloadImmediate]);

	return (
		<>
			<Helmet>
				<title>Game | Kubrick</title>
			</Helmet>

			<div className={classes.Main}>
				<button className={classes.Back} onClick={() => navigate('/')}>
					<img src={back} alt='back' width='50' height='50' />
				</button>

				{!isLoaded && <p className={classes.Loading}>{Math.round(loadingProgression * 100)}%</p>}

				<Unity unityProvider={unityProvider} className={classes.Unity} />
			</div>
		</>
	);
};

export default Game;
