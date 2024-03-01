import React, { useState, useEffect } from 'react';
import './S3FileList.css'; // Import CSS file

const S3FileList = () => {
  const [files, setFiles] = useState([]);
  const [selectedFileContent, setSelectedFileContent] = useState('');
  const apiUrl = 'http://<URL>';
  useEffect(() => {
    fetchS3Files();
  }, []);

  const fetchS3Files = async () => {
    try {
      const response = await fetch(apiUrl+'/api/s3files/buckets');
      if (!response.ok) {
        throw new Error('Failed to fetch S3 files.');
      }
      const data = await response.json();
      setFiles(data);
    } catch (error) {
      console.error(error);
    }
  };

  const fetchS3FileContent = async (event) => {
    try {
      let fileName=event.currentTarget.getAttribute("data-value");
      let bucketname=event.currentTarget.getAttribute("data-value1");
      //alert(fileName)
      const response = await fetch(apiUrl+`/api/s3files/read?fileName=${fileName}`+"&bucketname="+bucketname);
      if (!response.ok) {
        throw new Error('Failed to fetch file content.');
      }
      const content = await response.text();
      setSelectedFileContent(content);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="s3-file-list">
      <h2>Click below to view messages by phone number</h2>
      <ul>
        {files.map((file) => (
          <li key={file.Key}>
            <button data-value={file.key} data-value1={file.bucketName}  onClick={ fetchS3FileContent}>+{file.key.split('.')[0]}</button>
          </li>
        ))}
      </ul>
      {selectedFileContent && (
        <div>
          <h3>Messages:</h3>
          <pre>{selectedFileContent}</pre>
        </div>
      )}
    </div>
  );
};

export default S3FileList;
