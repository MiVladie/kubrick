import React from 'react';

import classes from './Video.module.scss';

interface Props {
	title: string;
	url: string;
	className?: string;
}

const Video: React.FC<Props> = ({ title, url, className }) => {
	return (
		<div className={[classes.Container, className].join(' ')}>
			<iframe
				title={title}
				className={classes.Video}
				src={url}
				frameBorder='0'
				allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture'
				allowFullScreen
			/>
		</div>
	);
};

export default Video;
