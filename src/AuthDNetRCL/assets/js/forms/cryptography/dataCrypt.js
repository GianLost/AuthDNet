export async function encryptData(data, key, iv) {

    const enc = new TextEncoder();
    const encodedData = enc.encode(JSON.stringify(data));

    const cryptoKey = await crypto.subtle.importKey(
        'raw',
        key,
        { name: 'AES-CBC' },
        false,
        ['encrypt']
    );

    const encryptedData = await crypto.subtle.encrypt(
        { name: 'AES-CBC', iv },
        cryptoKey,
        encodedData
    );

    return btoa(String.fromCharCode(...new Uint8Array(encryptedData)));
}