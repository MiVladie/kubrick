export const isMobile = (): boolean => {
	const toMatch = [/Android/i, /webOS/i, /iPhone/i, /iPad/i, /iPod/i, /BlackBerry/i, /Windows Phone/i];

	return toMatch.some((toMatchItem) => {
		return navigator.userAgent.match(toMatchItem);
	});
};

export const openURL = (url: string, newTab = false) => {
	if (newTab) window.open(url, '_blank', 'noopener,noreferrer');
	else window.location.href = url;
};
